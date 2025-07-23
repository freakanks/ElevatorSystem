namespace ElevatorSystem.Entities
{
    public interface ICallRequest
    {
        int Floor { get; set; }
        Direction Direction { get; set; }
    }
}
