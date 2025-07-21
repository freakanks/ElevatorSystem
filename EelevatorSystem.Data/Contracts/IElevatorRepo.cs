using ElevatorSystem.Entities;

namespace EelevatorSystem.Data
{
    /// <summary>
    /// Repository for elevators operation.
    /// </summary>
    public interface IElevatorRepo
    {
        /// <summary>
        /// Calls nearest elevator to the floor provided for going in direction.
        /// </summary>
        /// <param name="floor"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        void CallElevator(int floor, Direction direction);
        /// <summary>
        /// Gets the information of all elevators along with their current state.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IElevator> GetElevatorsStatus();
    }
}
