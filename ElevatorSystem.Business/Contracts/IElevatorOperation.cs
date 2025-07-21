using ElevatorSystem.Entities;

namespace ElevatorSystem.Business
{
    public interface IElevatorOperation
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
