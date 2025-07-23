namespace ElevatorSystem.Entities
{
    public interface IElevator
    {
        int ID { get; set; }
        int CurrentFloor { get; set; }
        bool PickingUpPassenger { get; set; }
        Direction MovementDirection { get; set; }
        IReadOnlyCollection<int> Destinations { get; }
        void AddDestination(int floor);
        DateTime NextAvailableTime { get; set; }
        void RemoveDestination(int floor);
        int? NextDestination();
        bool CanMove();
    }
    public enum Direction
    {
        HALT,
        UP,
        DOWN
    }
}
