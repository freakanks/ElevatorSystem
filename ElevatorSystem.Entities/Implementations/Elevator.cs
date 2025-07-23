namespace ElevatorSystem.Entities
{
    public class Elevator : IElevator
    {
        public int ID { get; set; }
        public int CurrentFloor { get; set; } = 0;
        public Direction MovementDirection { get; set; } = Direction.HALT;
        private readonly SortedSet<int> _destinations = new();

        public DateTime NextAvailableTime { get; set; } = DateTime.Now;

        public IReadOnlyCollection<int> Destinations => _destinations;

        public bool PickingUpPassenger { get; set; }

        public Elevator(int id, int initialFloor = 1)
        {
            ID = id;
            CurrentFloor = initialFloor;
        }

        public void AddDestination(int floor)
        {
            _destinations.Add(floor);
            UpdateDirection();
        }

        public void RemoveDestination(int floor)
        {
            _destinations.Remove(floor);
            UpdateDirection();
        }

        public int? NextDestination() => _destinations.FirstOrDefault();

        private void UpdateDirection()
        {
            if (!_destinations.Any())
            {
                MovementDirection = Direction.HALT;
                return;
            }

            MovementDirection = _destinations.Max > CurrentFloor ? Direction.UP : Direction.DOWN;
        }

        public bool CanMove() => DateTime.Now >= NextAvailableTime;


    }
}
