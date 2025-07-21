using EelevatorSystem.Data;
using ElevatorSystem.Entities;
using System.Text.RegularExpressions;

namespace ElevatorSystem.Business
{
    ///<inheritdoc/>
    public class ElevatorOperation : IElevatorOperation
    {
        private readonly IElevatorRepo _repo;

        public ElevatorOperation(IElevatorRepo repo)
        {
            _repo = repo;
        }

        public void CallElevator(int floor, Direction direction)
        {
            try
            {
                _repo.CallElevator(floor, direction);
            }
            catch (Exception e)
            {
                //Do Logging
            }
        }

        public IEnumerable<IElevator> GetElevatorsStatus()
        {
            try
            {
                return _repo.GetElevatorsStatus();
            }
            catch (Exception e)
            {
                //Do Logging
            }
            return null;
        }
    }
}
