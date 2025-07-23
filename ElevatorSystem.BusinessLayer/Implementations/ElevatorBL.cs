using ElevatorSystem.DataLayer;
using ElevatorSystem.Entities;
using Microsoft.Extensions.Logging;
using System.Net.NetworkInformation;

namespace ElevatorSystem.BusinessLayer
{
    public class ElevatorBL : IElevatorBL
    {
        private readonly ILogger<ElevatorBL> _logger;
        private readonly IElevatorDL _dataLayer;

        public ElevatorBL(ILogger<ElevatorBL> logger, IElevatorDL dataLayer)
        {
            _logger = logger;
            _dataLayer = dataLayer;
        }

        public event Action? StateChanged;

        public void EnqueueCall(CallRequest request)
        {
            try
            {
                if (_dataLayer.ElevatorsCallQueue.Any(x => x.Floor == request.Floor) ||
                    _dataLayer.Elevators.SelectMany(x => x.Destinations).Contains(request.Floor))
                    return;
                _dataLayer.EnqueueCall(request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Occured while enqueuing call for {request.Floor} and to go to {request.Direction} : {ex}");
            }
        }

        public List<IElevator> GetElevatorsCollection() => _dataLayer.Elevators;

        void MoveOneFloor(IElevator elevator)
        {
            if (!elevator.CanMove())
            {
                if (elevator.PickingUpPassenger)
                    _logger.LogInformation($"Elevator {elevator.ID} is on floor {elevator.CurrentFloor} and will move after {(elevator.NextAvailableTime - DateTime.Now).TotalSeconds} seconds");
                return;
            }
            elevator.PickingUpPassenger = false;
            var next = elevator.NextDestination();
            if (next == null) return;

            elevator.CurrentFloor = elevator.CurrentFloor < next ?
                                                    elevator.CurrentFloor + 1 : elevator.CurrentFloor > next ?
                                                            elevator.CurrentFloor - 1 : elevator.CurrentFloor;
            elevator.NextAvailableTime = DateTime.Now.AddSeconds(10);

            if (elevator.CurrentFloor == next)
            {
                elevator.RemoveDestination(next.Value);
                _logger.LogInformation($"Elevator {elevator.ID} arrived at floor {elevator.CurrentFloor} and picking up passengers");
                elevator.PickingUpPassenger = true;
                elevator.NextAvailableTime = DateTime.Now.AddSeconds(10);
            }
        }

        public bool SynchronizeElevators()
        {
            try
            {
                var changed = false;
                while (_dataLayer.ElevatorsCallQueue.TryPeek(out var call))
                {
                    var elevator = _dataLayer.SelectElevator(call);
                    if (elevator is null) break;

                    _dataLayer.DequeueCall();
                    elevator.AddDestination(call.Floor);
                    _logger.LogInformation($"Assigned floor {call.Floor} to elevator {elevator.ID}");
                }

                foreach (var elevator in _dataLayer.Elevators)
                {
                    if (elevator.MovementDirection == Direction.HALT) continue;

                    var before = elevator.CurrentFloor;
                    MoveOneFloor(elevator);
                    if (elevator.CurrentFloor != before)
                    {
                        changed = true;
                        if (!elevator.PickingUpPassenger)
                            _logger.LogInformation($"Elevator {elevator.ID} arrived at floor {elevator.CurrentFloor}");
                    }
                }
                if (changed)
                    StateChanged?.Invoke();
                return changed;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error occured while synching the elevators {e}");
            }
            return false;
        }
    }
}
