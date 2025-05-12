using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TicketsApp.Models;

namespace TicketsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger; // Logger for logging errors and messages

        // Constructor to initialize the logger
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // GET: Home/Index - Returns the home page view
        public IActionResult Index()
        {
            return View(); // Return the view for the home page
        }

        // GET: Home/Privacy - Returns the privacy page view
        public IActionResult Privacy()
        {
            return View(); // Return the privacy page view
        }

        // GET: Home/Error - Handles errors and displays the error page
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] // Disables caching for the error page
        public IActionResult Error()
        {
            // Creates an ErrorViewModel with the current request ID or trace identifier
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
