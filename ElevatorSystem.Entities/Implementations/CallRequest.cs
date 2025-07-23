namespace ElevatorSystem.Entities
{
    public class CallRequest : ICallRequest
    {
        public int Floor { get; set; }
        public Direction Direction { get; set; }
    }
}
