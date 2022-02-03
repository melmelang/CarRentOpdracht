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
            HttpContext.Session.SetString("title", "Hey " + _user.UserName + ", dit zijn de gebruikte Bronnen");

            var isTrue = _context.Users.Where(u => u.Id == _user.Id)
                                        .Select(u => u.AcceptCookie);
            foreach (bool item in isTrue)
            {
                if (item)
                {
                    Cookie c = new Cookie("Bronnen", "Om de bronnen te zien moet je op Bronnen/References gaan en op de link klikken");
                    ViewData["Bronnen"] = c.Value;
                } else
                {
                    ViewData["Bronnen"] = "Cookies zijn momenteel uitgeschakeld. Om ze aan te zetten ga naar uw profiel en aanvaard ze.";
                }
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