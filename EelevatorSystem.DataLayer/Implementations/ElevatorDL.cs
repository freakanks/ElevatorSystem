using ElevatorSystem.Entities;
using Microsoft.Extensions.Logging;

namespace ElevatorSystem.DataLayer
{
    ///<inheritdoc/>
    public class ElevatorDL : IElevatorDL
    {
        private const int Floors = 10;
        private const int ElevatorCount = 4;
        private readonly ILogger<ElevatorDL> _logger;
        private readonly Queue<CallRequest> _callQueue = new();

        public List<IElevator> Elevators { get; } = new(ElevatorCount);

        public Queue<CallRequest> ElevatorsCallQueue => _callQueue;

        public ElevatorDL(ILogger<ElevatorDL> logger)
        {
            for (var i = 0; i < ElevatorCount; i++)
            {
                Elevators.Add(new Elevator(i + 1));
            }
            _logger = logger;
        }

        public void EnqueueCall(CallRequest request)
        {
            _callQueue.Enqueue(request);
            _logger.LogInformation("Elevator Call enqueued: {Floor} {Direction}", request.Floor, request.Direction);
        }

        public void DequeueCall() => _callQueue.Dequeue();

        public IElevator? SelectElevator(CallRequest call)
        {

            var alongTheWay = Elevators.FirstOrDefault(e =>
                e.MovementDirection == call.Direction &&
                ((call.Direction == Direction.UP && e.CurrentFloor <= call.Floor) ||
                 (call.Direction == Direction.DOWN && e.CurrentFloor >= call.Floor)));
            if (alongTheWay != null) return alongTheWay;

            var halted = Elevators
                .Where(e => e.MovementDirection == Direction.HALT)
                .OrderBy(e => Math.Abs(e.CurrentFloor - call.Floor))
                .FirstOrDefault();
            return halted;
        }
    }
}
