using ElevatorSystem.Entities;

namespace ElevatorSystem.BusinessLayer
{
    public interface IElevatorBL
    {
        /// <summary>
        /// Enqueues Call to call nearest elevator to the floor provided for going in direction.
        /// </summary>
        /// <param name="floor"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        void EnqueueCall(CallRequest request);
        /// <summary>
        /// Event to attach for any state change in eleavtors.
        /// </summary>
        /// <returns></returns>
        event Action? StateChanged;
        /// <summary>
        /// Peridiocally Synchronize elevators.
        /// </summary>
        /// <returns></returns>
        bool SynchronizeElevators();
        /// <summary>
        /// Gets all Elevators configured.
        /// </summary>
        List<IElevator> GetElevatorsCollection();
    }
}
