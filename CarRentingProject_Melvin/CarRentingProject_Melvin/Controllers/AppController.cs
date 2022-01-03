using CarRentingProject_Melvin.Areas.Identity.Data;
using CarRentingProject_Melvin.Data;
using CarRentingProject_Melvin.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRentingProject_Melvin.Controllers
{
    public class AppController : Controller
    {
        protected readonly CarRentingProject_AppUser _user;
        protected readonly DBContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly ILogger<AppController> _logger;

        protected AppController(DBContext context,
                                IHttpContextAccessor httpContextAccessor,
                                ILogger<AppController> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;

            _user = SessionUser.GetUser(httpContextAccessor.HttpContext);
        }
    }
}
