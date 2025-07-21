using ElevatorSystem.Entities;
using Microsoft.Extensions.Hosting;

namespace ElevatorSystem.Business.Implementations
{
    public class RandomOperator : IHostedService
    {
        private readonly IElevatorOperation _elevatorOperation;
        private Timer _timer;
        private Random _random = new Random();
        public RandomOperator(IElevatorOperation elevatorOperation)
        {
            _elevatorOperation = elevatorOperation;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (_timer == null)
                _timer = new Timer((state) => RandomlyCallElevator(), null, 1000, 10 * 1000);
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Dispose();
            await Task.CompletedTask;
        }

        private void RandomlyCallElevator()
        {
            var floor = _random.Next(1, 10);
            var direction = _random.Next(2) == 0 ? Direction.DOWN : Direction.UP;
            _elevatorOperation.CallElevator(floor, direction);
        }
    }
}
