using Microsoft.AspNetCore.Mvc;
using CarRentingProject_Melvin.Models;
using System.Diagnostics;
using CarRentingProject_Melvin.Data;

namespace CarRentingProject_Melvin.Controllers
{
    public class HomeController : AppController
    {
        //private readonly ILogger<HomeController> _logger;

        public HomeController(DBContext context, IHttpContextAccessor httpContextAccessor,
                                    ILogger<AppController> logger) : base(context, httpContextAccessor, logger)
        {
        }

        public IActionResult Index()
        {
            return View();
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