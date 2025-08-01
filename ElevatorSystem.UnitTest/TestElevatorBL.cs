using ElevatorSystem.BusinessLayer;
using ElevatorSystem.DataLayer;
using ElevatorSystem.Entities;
using Microsoft.Extensions.Logging.Abstractions;

namespace ElevatorSystem.UnitTest
{
    public class TestElevatorBL
    {
        private IElevatorDL GetElevatorDL() => new ElevatorDL(new NullLogger<ElevatorDL>());
        private IElevatorBL GetElevatorBL() => new ElevatorBL(new NullLogger<ElevatorBL>(), GetElevatorDL());
        [Fact]
        public void WhenRequestElevator_IdleElevatorAssigned()
        {
            var businessLayer = GetElevatorBL();

            businessLayer.EnqueueCall(new CallRequest { Floor = 5, Direction = Direction.UP });
            businessLayer.SynchronizeElevators();

            var elevator = businessLayer.GetElevatorsCollection().First();
            Assert.Contains(5, elevator.Destinations);
        }

        [Fact]
        public void WhenAssignedFloor_ElevatorShouldReachDestination()
        {
            var businessLayer = GetElevatorBL();

            businessLayer.EnqueueCall(new CallRequest { Floor = 3, Direction = Direction.UP });
            businessLayer.SynchronizeElevators();
            var elevator = businessLayer.GetElevatorsCollection().First();

            while (elevator.CurrentFloor != 3)
            {
                elevator.NextAvailableTime = DateTime.Now.AddSeconds(-1);
                businessLayer.SynchronizeElevators();
            }

            Assert.Equal(3, elevator.CurrentFloor);
            Assert.Equal(Direction.HALT, elevator.MovementDirection);
        }

        [Fact]
        public void WhenFloorArrives_ElevatorShouldHalt()
        {
            var businessLayer = GetElevatorBL();
            var elevator = businessLayer.GetElevatorsCollection().First();

            businessLayer.EnqueueCall(new CallRequest() { Floor = 3, Direction = Direction.UP });
            businessLayer.SynchronizeElevators();

            var initialTime = elevator.NextAvailableTime;
            Assert.True(elevator.NextAvailableTime > DateTime.Now);

            elevator.NextAvailableTime = DateTime.Now.AddSeconds(-1);
            businessLayer.SynchronizeElevators();

            Assert.True(elevator.NextAvailableTime > DateTime.Now);
        }

        [Fact]
        public void WhenAddedDestination_ElevatorShouldMoveInCorrectDirection()
        {
            var businessLayer = GetElevatorBL();
            var elevator = businessLayer.GetElevatorsCollection().First();
            elevator.CurrentFloor = 5;
            elevator.AddDestination(8);

            elevator.NextAvailableTime = DateTime.Now;
            businessLayer.SynchronizeElevators();

            Assert.Equal(6, elevator.CurrentFloor);
            Assert.Equal(Direction.UP, elevator.MovementDirection);
        }

        [Fact]
        public void WhenAddedDestination_ElevatorShouldStopAtCorrectFloorAndHalts()
        {
            var businessLayer = GetElevatorBL();
            var elevator = businessLayer.GetElevatorsCollection().First();
            elevator.CurrentFloor = 2;
            elevator.AddDestination(3);
            elevator.NextAvailableTime = DateTime.Now;

            businessLayer.SynchronizeElevators();
            Assert.Equal(3, elevator.CurrentFloor);
            Assert.True(elevator.NextAvailableTime > DateTime.Now);
            Assert.Equal(Direction.HALT, elevator.MovementDirection);
        }

        [Fact]
        public void WhenElevatorPcikingPassengers_ShoudlIgnoreMoveBeforeAvailableTime()
        {
            var businessLayer = GetElevatorBL();
            var elevator = businessLayer.GetElevatorsCollection().First();
            elevator.CurrentFloor = 2;
            elevator.AddDestination(4);
            elevator.NextAvailableTime = DateTime.Now.AddSeconds(10);

            businessLayer.SynchronizeElevators();
            Assert.Equal(2, elevator.CurrentFloor);
        }

        [Fact]
        public void WhenElevatorRequested_ShouldAssignToElevatorGoingSameDirection()
        {
            var businessLayer = GetElevatorBL();
            var elevator = businessLayer.GetElevatorsCollection().First();
            elevator.CurrentFloor = 4;
            elevator.AddDestination(7);

            businessLayer.EnqueueCall(new CallRequest() { Floor = 6, Direction = Direction.UP });
            businessLayer.SynchronizeElevators();

            Assert.Contains(6, elevator.Destinations);
        }
    }
}