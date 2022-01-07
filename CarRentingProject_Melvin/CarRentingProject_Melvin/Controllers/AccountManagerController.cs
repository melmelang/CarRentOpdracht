using CarRentingProject_Melvin.Areas.Identity.Data;
using CarRentingProject_Melvin.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CarRentingProject_Melvin.Models;

namespace CarRentingProject_Melvin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccountManagerController : AppController
    {
        public AccountManagerController(DBContext context,
            IHttpContextAccessor httpContextAccessor,
            ILogger<AppController> logger) : base(context, httpContextAccessor, logger)
        {
        }

        public IActionResult Index(string userName, string name, string email, int? pageNumber)
        {
            if (userName == null) userName = "";
            if (name == null) name = "";
            if (email == null) email = "";
            List<CarRentingProject_AppUser> users =
                _context.Users.ToList()
                .Where(u => (userName == "" || u.UserName.Contains(userName))
                         && (name == "" || (u.FirstName.Contains(name) || u.LastName.Contains(name)))
                         && (email == "" || u.Email.Contains(email)))
                .OrderBy(u => u.FirstName + " " + u.LastName)
                .ToList();
            List<AppUserViewModel> userViewModels = new List<AppUserViewModel>();
            foreach (var user in users)
            {
                userViewModels.Add(new AppUserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Birthday = user.Birthday,
                    GenderId = user.GenderId,
                    Lockout = user.LockoutEnd != null,
                    PhoneNumber = user.PhoneNumber,
                    Tenant = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Tenant").Count() > 0,
                    Renter = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Renter").Count() > 0,
                    Admin = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Admin").Count() > 0
                });

            }
            ViewData["userName"] = userName;
            ViewData["name"] = name;
            ViewData["email"] = email;
            if (pageNumber == null) pageNumber = 1;
            PageList<AppUserViewModel> model = new PageList<AppUserViewModel>(userViewModels, userViewModels.Count, 1, 10);
            return View(model);
        }

        public async Task<ActionResult> Locking(string id)
        {
            CarRentingProject_AppUser user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user.LockoutEnd != null)
                user.LockoutEnd = null;
            else
                user.LockoutEnd = new DateTimeOffset(DateTime.Now + new TimeSpan(7, 0, 0, 0));
            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ActionResult Roles(string id)
        {
            CarRentingProject_AppUser user = _context.Users.FirstOrDefault(u => u.Id == id);
            AppUserViewModel model = new AppUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthday = user.Birthday,
                GenderId = user.GenderId,
                Lockout = user.LockoutEnd != null,
                PhoneNumber = user.PhoneNumber,
                Tenant = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Tenant").Count() > 0,
                Renter = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Renter").Count() > 0,
                Admin = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Admin").Count() > 0
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Roles([Bind("Id, UserName, FirstName, LastName, Tenant, Birthday, GenderId, Renter, Admin")] AppUserViewModel model)
        {
            List<IdentityUserRole<string>> roles = _context.UserRoles.Where(ur => ur.UserId == model.Id).ToList();
            foreach (IdentityUserRole<string> role in roles)
            {
                _context.Remove(role);
            }
            if (model.Tenant) _context.Add(new IdentityUserRole<string> { RoleId = "Tenant", UserId = model.Id });
            if (model.Renter) _context.Add(new IdentityUserRole<string> { RoleId = "Renter", UserId = model.Id });
            if (model.Admin) _context.Add(new IdentityUserRole<string> { RoleId = "Admin", UserId = model.Id });
            await _context.SaveChangesAsync();
            ; return RedirectToAction("Index");
        }
    }

}
