using CarRentingProject_Melvin.Data;
using CarRentingProject_Melvin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

namespace CarRentingProject_Melvin.Controllers
{
    public class HomeController : AppController
    {

        public HomeController(DBContext context, IHttpContextAccessor httpContextAccessor,
                                    ILogger<AppController> logger) : base(context, httpContextAccessor, logger)
        {
        }

        public IActionResult Index()
        {
            if (_user.AcceptCookie)
            {
                Cookie c = new Cookie();
                c.Name = "test";
                c.Value = "test the test";
                c.Expires = DateTime.MaxValue;
                c.Expired = false;
                c.Secure = true;
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}