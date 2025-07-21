using ElevatorSystem.Business;
using ElevatorSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ElevatorSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IElevatorOperation _elevatorOperation;

        public HomeController(ILogger<HomeController> logger, IElevatorOperation elevatorOperation)
        {
            _logger = logger;
            _elevatorOperation = elevatorOperation;
        }

        public IActionResult Index()
        {
            var elevators = _elevatorOperation.GetElevatorsStatus();
            return View(elevators);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
