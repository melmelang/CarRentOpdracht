#nullable disable
using CarRentingProject_Melvin.Areas.Identity.Data;
using CarRentingProject_Melvin.Data;
using CarRentingProject_Melvin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarRentingProject_Melvin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RentersController : AppController
    {
        private readonly UserManager<CarRentingProject_AppUser> _userManager;

        public RentersController(UserManager<CarRentingProject_AppUser> userManager,
                                    DBContext context, IHttpContextAccessor httpContextAccessor,
                                    ILogger<AppController> logger) : base(context, httpContextAccessor, logger)
        {
            _userManager = userManager;
        }

        // GET: Renters
        public async Task<IActionResult> Index()
        {
            var dBContext = _context.Renter.Include(r => r.Gender);
            return View(await dBContext.ToListAsync());
        }

        // GET: Renters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var renter = await _context.Renter
                .Include(r => r.Gender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (renter == null)
            {
                return NotFound();
            }

            return View(renter);
        }

        // GET: Renters/Create
        public IActionResult Create()
        {
            ViewData["GenderId"] = new SelectList(_context.Gender, "Id", "Name");
            return View();
        }

        // POST: Renters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,UserName,Birthday,GenderId,UserId")] Renter renter)
        {
            if (ModelState.IsValid)
            {
                var user = Activator.CreateInstance<CarRentingProject_AppUser>();
                user.FirstName = renter.FirstName;
                user.LastName = renter.LastName;
                user.UserName = renter.UserName;
                user.GenderId = renter.GenderId;
                user.Birthday = renter.Birthday;
                user.Email = renter.UserName + "@renter.be";
                user.EmailConfirmed = true;
                await _userManager.CreateAsync(user, renter.FirstName + "." + renter.LastName + "REN1");

                renter.UserId = user.Id;
                _context.Add(renter);
                await _context.SaveChangesAsync();
                await _userManager.AddToRoleAsync(user, "Renter");
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenderId"] = new SelectList(_context.Gender, "Id", "Name", renter.GenderId);
            return View(renter);
        }

        // GET: Renters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var renter = await _context.Renter.FindAsync(id);
            if (renter == null)
            {
                return NotFound();
            }
            ViewData["GenderId"] = new SelectList(_context.Gender, "Id", "Name", renter.GenderId);
            return View(renter);
        }

        // POST: Renters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,UserName,Birthday,GenderId")] Renter renter)
        {
            if (id != renter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(renter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RenterExists(renter.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenderId"] = new SelectList(_context.Gender, "Id", "Name", renter.GenderId);
            return View(renter);
        }

        // GET: Renters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var renter = await _context.Renter
                .Include(r => r.Gender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (renter == null)
            {
                return NotFound();
            }

            return View(renter);
        }

        // POST: Renters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var renter = await _context.Renter.FindAsync(id);
            _context.Renter.Remove(renter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RenterExists(int id)
        {
            return _context.Renter.Any(e => e.Id == id);
        }
    }
}
