using ElevatorSystem.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ElevatorSystem.BusinessLayer
{
    public class SimulationService : BackgroundService
    {
        private readonly ILogger<SimulationService> _logger;
        private readonly IHubContext<ElevatorHub> _hub;
        private readonly IElevatorBL _businessLayer;
        public SimulationService(IHubContext<ElevatorHub> hub, IElevatorBL repo, ILogger<SimulationService> logger)
        {
            _hub = hub;
            _businessLayer = repo;
            _businessLayer.StateChanged += async () =>
            {
                await _hub.Clients.All.SendAsync("StateUpdated", _businessLayer.GetElevatorsCollection().Select(e => new
                {
                    e.ID,
                    e.CurrentFloor,
                    Direction = e.MovementDirection.ToString(),
                    Destinations = e.Destinations.ToArray()
                }));
            };
            _logger = logger;
        }

        private readonly Random _rnd = new();

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Simulation Service Started to operate eleavtors.");
            var timerForElevatorReuqest = new Timer((state) =>
            {
                var floor = _rnd.Next(1, 11);
                var dir = floor == 1 ? Direction.UP : floor == 10 ? Direction.DOWN : _rnd.NextDouble() < 0.5 ? Direction.UP : Direction.DOWN;
                _logger.LogInformation($"Sending Request to call elevator to Floor {floor} to go {dir}");
                _businessLayer.EnqueueCall(new CallRequest { Floor = floor, Direction = dir });

            }, null, 10000, 10000);
            
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Simulation Service Stopped.");
                    break;
                }
                _businessLayer.SynchronizeElevators();
            }
            if(stoppingToken.IsCancellationRequested)
                timerForElevatorReuqest.Dispose();
        }
    }
}
