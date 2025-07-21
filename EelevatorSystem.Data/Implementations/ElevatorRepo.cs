using ElevatorSystem.Entities;

namespace EelevatorSystem.Data
{
    ///<inheritdoc/>
    public class ElevatorRepo : IElevatorRepo
    {
        private Timer? timer;
        /// <summary>
        /// Dictionary for holding all the eleavtors data.
        /// </summary>
        private List<IElevator> Elevators = new List<IElevator>()
        {
            new Elevator(){ ID = 1 },
            new Elevator(){ ID = 2 },
            new Elevator(){ ID = 3 },
            new Elevator(){ ID = 4 },
        };
        private object _lock = new object();

        /// <summary>
        /// Keeps on opearting the elevator in background
        /// </summary>
        private void OperateElevators()
        {
            var currentDateTime = DateTime.Now;
            foreach (var elevator in Elevators)
            {
                if (elevator.MovementDirection != Direction.HALT && elevator.FloorsToHalt.Any() && (elevator.NextFloorReachingTime ?? currentDateTime) <= currentDateTime)
                {
                    elevator.CurrentFloor = elevator.MovementDirection == Direction.UP ? elevator.CurrentFloor + 1 :
                                                elevator.MovementDirection == Direction.DOWN ? elevator.CurrentFloor - 1 : elevator.CurrentFloor;
                    if (elevator.FloorsToHalt.Contains(elevator.CurrentFloor))
                    {
                        elevator.FloorsToHalt.Remove(elevator.CurrentFloor);
                        if (elevator.FloorsToHalt.Any())
                            elevator.NextFloorReachingTime = currentDateTime.AddSeconds(10);
                        else
                        {
                            elevator.NextFloorReachingTime = null;
                            elevator.MovementDirection = Direction.HALT;
                        }
                    }
                    if (elevator.MovementDirection != Direction.HALT)
                        elevator.NextFloorReachingTime = elevator.NextFloorReachingTime.HasValue ?
                                                                elevator.NextFloorReachingTime.Value.AddSeconds(10) :
                                                                        currentDateTime.AddSeconds(10);
                }
            }
        }

        public ElevatorRepo()
        {
            timer = new Timer((state) => OperateElevators(), null, 1000, 1000);
        }

        public void CallElevator(int floor, Direction direction)
        {
            lock (_lock)
            {
                if (direction == Direction.UP)
                {
                    var elevator = Elevators.OrderBy(a => Math.Abs(a.CurrentFloor - floor))
                                            .FirstOrDefault(x => x.MovementDirection == Direction.UP || x.MovementDirection == Direction.HALT);
                    if (elevator != null)
                    {
                        elevator.MovementDirection = elevator.CurrentFloor > floor ? Direction.DOWN : Direction.UP;
                        elevator.FloorsToHalt.Add(floor);
                    }
                }
                else if (direction == Direction.DOWN)
                {
                    var elevator = Elevators.OrderBy(a => Math.Abs(a.CurrentFloor - floor))
                                            .FirstOrDefault(x => x.MovementDirection == Direction.DOWN || x.MovementDirection == Direction.HALT);
                    if (elevator != null)
                    {
                        elevator.MovementDirection = elevator.CurrentFloor < floor ? Direction.UP : Direction.DOWN;
                        elevator.FloorsToHalt.Add(floor);
                    }
                }
            }
        }

        public IEnumerable<IElevator> GetElevatorsStatus() => Elevators;
    }
}
