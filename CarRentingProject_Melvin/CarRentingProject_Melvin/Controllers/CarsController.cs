#nullable disable
using CarRentingProject_Melvin.Data;
using CarRentingProject_Melvin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarRentingProject_Melvin.Controllers
{
    public class CarsController : AppController
    {

        public CarsController(DBContext context, IHttpContextAccessor httpContextAccessor,
                                    ILogger<AppController> logger) : base(context, httpContextAccessor, logger)
        {
        }

        // GET: Cars
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Renter"))
            {
                var dBContext = _context.Cars.Include(c => c.Renter).Where(c => c.Renter.UserId == _user.Id);
                return View(await dBContext.ToListAsync());
            }
            else
            {
                var dBContext = _context.Cars.Include(c => c.Renter);
                return View(await dBContext.ToListAsync());
            }
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cars = await _context.Cars
                .Include(c => c.Renter)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cars == null)
            {
                return NotFound();
            }

            return View(cars);
        }

        [Authorize(Roles = "Renter")]
        // GET: Cars/Create
        public IActionResult Create()
        {
            ViewData["RenterId"] = new SelectList(_context.Set<Renter>(), "Id", "FirstName");
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Renter")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Brand,Model,Price,ProductionDate,Mileage,RenterId")] Cars cars)
        {
            var renterId = _context.Renter.Where(r => r.UserId == _user.Id).Select(r => r.Id);
            if (ModelState.IsValid)
            {
                foreach (var ren in renterId)
                {
                    cars.RenterId = ren;
                }
                _context.Add(cars);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RenterId"] = new SelectList(_context.Set<Renter>(), "Id", "FirstName", cars.RenterId);
            return View(cars);
        }

        [Authorize(Roles = "Renter,Admin")]
        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cars = await _context.Cars.FindAsync(id);
            if (cars == null)
            {
                return NotFound();
            }
            ViewData["RenterId"] = new SelectList(_context.Set<Renter>(), "Id", "FirstName", cars.RenterId);
            return View(cars);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Renter,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Brand,Model,Price,ProductionDate,Mileage,RenterId")] Cars cars)
        {
            if (id != cars.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cars);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarsExists(cars.Id))
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
            ViewData["RenterId"] = new SelectList(_context.Set<Renter>(), "Id", "FirstName", cars.RenterId);
            return View(cars);
        }

        [Authorize(Roles = "Renter,Admin")]
        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cars = await _context.Cars
                .Include(c => c.Renter)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cars == null)
            {
                return NotFound();
            }

            return View(cars);
        }

        [Authorize(Roles = "Renter,Admin")]
        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cars = await _context.Cars.FindAsync(id);
            _context.Cars.Remove(cars);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarsExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }
    }
}
