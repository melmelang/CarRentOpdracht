#nullable disable
using CarRentingProject_Melvin.Data;
using CarRentingProject_Melvin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarRentingProject_Melvin.Controllers
{
    [Authorize]
    public class RentedCarsController : AppController
    {

        public RentedCarsController(DBContext context, IHttpContextAccessor httpContextAccessor,
                                    ILogger<AppController> logger) : base(context, httpContextAccessor, logger)
        {
        }

        // GET: RentedCars
        public async Task<IActionResult> Index()
        {
            var RentedCars = _context.RentedCars.Where(r => r.RideTime > DateTime.Now);
            foreach (var rentedCars in RentedCars)
            {
                _context.RentedCars.Remove(rentedCars);
            }
            if (User.IsInRole("Renter"))
            {
                var RenterId = _context.Renter.Where(r => r.UserId == _user.Id).Select(r => r.Id);
                var dBContext = _context.RentedCars.Include(r => r.Cars).Include(r => r.Tenant).Where(r => RenterId.Contains(r.Cars.RenterId));
                return View(await dBContext.ToListAsync());
            }
            else if (User.IsInRole("Tenant"))
            {
                var dBContext = _context.RentedCars.Include(r => r.Cars).Include(r => r.Tenant).Where(r => r.Tenant.UserId == _user.Id);
                return View(await dBContext.ToListAsync());
            }
            else
            {
                var dBContext = _context.RentedCars.Include(r => r.Cars).Include(r => r.Tenant);
                return View(await dBContext.ToListAsync());
            }
        }

        // GET: RentedCars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentedCars = await _context.RentedCars
                .Include(r => r.Cars)
                .Include(r => r.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentedCars == null)
            {
                return NotFound();
            }

            return View(rentedCars);
        }

        [Authorize(Roles = "Tendant")]
        // GET: RentedCars/Create
        public IActionResult Create(int? id, int? wich)
        {
            ViewData["CarsId"] = new SelectList(_context.Cars, "Id", "Brand");
            //ViewData["TenantId"] = new SelectList(_context.Tenant, "Id", "FirstName");
            if (wich == 2)
            {
                var cars = _context.Cars.Where(c => c.Id == id);
                ViewData["CarsId"] = new SelectList(cars, "Id", "Brand");
            }
            return View();
        }

        // POST: RentedCars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Tendant")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id, int? wich, [Bind("Id,CarsId,TenantId,RideTime")] RentedCars rentedCars)
        {
            var tenant = _context.Tenant.Where(t => t.UserId == _user.Id).Select(t => t.Id);
            if (ModelState.IsValid)
            {
                foreach (var item in tenant)
                {
                    rentedCars.TenantId = item;
                }
                _context.Add(rentedCars);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarsId"] = new SelectList(_context.Cars, "Id", "Brand", rentedCars.CarsId);
            //ViewData["TenantId"] = new SelectList(_context.Tenant, "Id", "FirstName", rentedCars.TenantId);
            return View(rentedCars);
        }

        [Authorize(Roles = "Tendant,Admin")]
        // GET: RentedCars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentedCars = await _context.RentedCars.FindAsync(id);
            if (rentedCars == null)
            {
                return NotFound();
            }
            ViewData["CarsId"] = new SelectList(_context.Cars, "Id", "Brand", rentedCars.CarsId);
            ViewData["TenantId"] = new SelectList(_context.Tenant, "Id", "FirstName", rentedCars.TenantId);
            return View(rentedCars);
        }

        // POST: RentedCars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Tendant,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CarsId,TenantId,RideTime")] RentedCars rentedCars)
        {
            if (id != rentedCars.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rentedCars);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentedCarsExists(rentedCars.Id))
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
            ViewData["CarsId"] = new SelectList(_context.Cars, "Id", "Brand", rentedCars.CarsId);
            ViewData["TenantId"] = new SelectList(_context.Tenant, "Id", "FirstName", rentedCars.TenantId);
            return View(rentedCars);
        }

        [Authorize(Roles = "Tendant,Admin")]
        // GET: RentedCars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentedCars = await _context.RentedCars
                .Include(r => r.Cars)
                .Include(r => r.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentedCars == null)
            {
                return NotFound();
            }

            return View(rentedCars);
        }

        // POST: RentedCars/Delete/5
        [Authorize(Roles = "Tendant,Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rentedCars = await _context.RentedCars.FindAsync(id);
            _context.RentedCars.Remove(rentedCars);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentedCarsExists(int id)
        {
            return _context.RentedCars.Any(e => e.Id == id);
        }
    }
}
