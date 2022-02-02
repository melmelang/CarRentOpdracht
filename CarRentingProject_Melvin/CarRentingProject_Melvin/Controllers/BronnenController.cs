using CarRentingProject_Melvin.Data;
using Microsoft.AspNetCore.Mvc;

namespace CarRentingProject_Melvin.Controllers
{
    public class BronnenController : AppController
    {

        public BronnenController(DBContext context, IHttpContextAccessor httpContextAccessor,
                                    ILogger<AppController> logger) : base(context, httpContextAccessor, logger)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Bronnen()
        {
            return View();
        }

    }
}