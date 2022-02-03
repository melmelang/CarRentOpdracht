using CarRentingProject_Melvin.Areas.Identity.Data;
using CarRentingProject_Melvin.Data;
using CarRentingProject_Melvin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult Index(string userName, string firstAndLastNeme, string email, int? pageNumber)
        {
            if (userName == null) userName = "";
            if (firstAndLastNeme == null) firstAndLastNeme = "";
            if (email == null) email = "";
            List<CarRentingProject_AppUser> appUsers =
                _context.Users.ToList()
                .Where(u => (userName == "" || u.UserName.Contains(userName))
                         && (firstAndLastNeme == "" || (u.FirstName.Contains(firstAndLastNeme) || u.LastName.Contains(firstAndLastNeme)))
                         && (email == "" || u.Email.Contains(email)))
                .OrderBy(u => u.FirstName + " " + u.LastName)
                .ToList();
            List<AppUserViewModel> userViewModels = new List<AppUserViewModel>();
            foreach (var u in appUsers)
            {
                userViewModels.Add(new AppUserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Birthday = u.Birthday,
                    GenderId = u.GenderId,
                    Lockout = u.LockoutEnd != null,
                    PhoneNumber = u.PhoneNumber,
                    Tenant = _context.UserRoles.Where(ur => ur.UserId == u.Id && ur.RoleId == "Tenant").Count() > 0,
                    Renter = _context.UserRoles.Where(ur => ur.UserId == u.Id && ur.RoleId == "Renter").Count() > 0,
                    Admin = _context.UserRoles.Where(ur => ur.UserId == u.Id && ur.RoleId == "Admin").Count() > 0
                });

            }
            ViewData["userName"] = userName;
            ViewData["name"] = firstAndLastNeme;
            ViewData["email"] = email;
            if (pageNumber == null) pageNumber = 1;
            PageList<AppUserViewModel> m = new PageList<AppUserViewModel>(userViewModels, userViewModels.Count, 1, 10);
            return View(m);
        }

        public async Task<ActionResult> Locking(string id)
        {
            CarRentingProject_AppUser appUser = _context.Users.FirstOrDefault(u => u.Id == id);
            if (appUser.LockoutEnd != null)
                appUser.LockoutEnd = null;
            else
                appUser.LockoutEnd = new DateTimeOffset(DateTime.Now + new TimeSpan(7, 0, 0, 0));
            _context.Update(appUser);
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
        public async Task<ActionResult> Roles([Bind("Id, UserName, FirstName, LastName, Tenant, Birthday, GenderId, Renter, Admin")] AppUserViewModel m)
        {
            List<IdentityUserRole<string>> permitions = _context.UserRoles.Where(p => p.UserId == m.Id).ToList();
            foreach (IdentityUserRole<string> p in permitions)
            {
                _context.Remove(p);
            }
            if (m.Tenant) _context.Add(new IdentityUserRole<string> { RoleId = "Tenant", UserId = m.Id });
            if (m.Renter) _context.Add(new IdentityUserRole<string> { RoleId = "Renter", UserId = m.Id });
            if (m.Admin) _context.Add(new IdentityUserRole<string> { RoleId = "Admin", UserId = m.Id });
            await _context.SaveChangesAsync();
            ; return RedirectToAction("Index");
        }
    }

}
