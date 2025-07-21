namespace ElevatorSystem.Entities
{
    public class Elevator : IElevator
    {
        public int ID { get; set; }
        public int CurrentFloor { get; set; } = 0;
        public Direction MovementDirection { get; set; }
        public List<int> FloorsToHalt { get; set; } = new List<int>();
        public DateTime? NextFloorReachingTime { get; set; }
    }
}
