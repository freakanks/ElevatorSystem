namespace ElevatorSystem.Entities
{
    public interface IElevator
    {
        int ID { get; set; }
        int CurrentFloor { get; set; }
        Direction MovementDirection { get; set; }
        List<int> FloorsToHalt { get; set; }
        DateTime? NextFloorReachingTime { get; set; }
    }
    public enum Direction
    {
        HALT,
        UP,
        DOWN
    }
}
