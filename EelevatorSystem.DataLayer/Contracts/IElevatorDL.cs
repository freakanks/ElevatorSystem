using ElevatorSystem.Entities;

namespace ElevatorSystem.DataLayer
{
    /// <summary>
    /// Repository for elevators operation.
    /// </summary>
    public interface IElevatorDL
    {
        /// <summary>
        /// Enqueues Call to call nearest elevator to the floor provided for going in direction.
        /// </summary>
        /// <param name="floor"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        void EnqueueCall(CallRequest request);
        /// <summary>
        /// Deques call for the elevator.
        /// </summary>
        /// <param name="request"></param>
        void DequeueCall();
        /// <summary>
        /// All Elevators configured.
        /// </summary>
        List<IElevator> Elevators { get; }
        /// <summary>
        /// Queue for elevators
        /// </summary>
        Queue<CallRequest> ElevatorsCallQueue { get; }
        /// <summary>
        /// Returns the closest elevator for the floor to fgo in direction.
        /// </summary>
        /// <param name="call"></param>
        /// <returns></returns>
        IElevator? SelectElevator(CallRequest call);
    }
}
