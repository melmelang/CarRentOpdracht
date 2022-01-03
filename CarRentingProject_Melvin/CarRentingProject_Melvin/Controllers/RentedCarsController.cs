#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarRentingProject_Melvin.Data;
using CarRentingProject_Melvin.Models;

namespace CarRentingProject_Melvin.Controllers
{
    public class RentedCarsController : Controller
    {
        private readonly DBContext _context;

        public RentedCarsController(DBContext context)
        {
            _context = context;
        }

        // GET: RentedCars
        public async Task<IActionResult> Index()
        {
            var dBContext = _context.RentedCars.Include(r => r.Cars).Include(r => r.Tenant);
            return View(await dBContext.ToListAsync());
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

        // GET: RentedCars/Create
        public IActionResult Create()
        {
            ViewData["CarsId"] = new SelectList(_context.Cars, "Id", "Brand");
            ViewData["TenantId"] = new SelectList(_context.Tenant, "Id", "FirstName");
            return View();
        }

        // POST: RentedCars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CarsId,TenantId,RideTime")] RentedCars rentedCars)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rentedCars);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarsId"] = new SelectList(_context.Cars, "Id", "Brand", rentedCars.CarsId);
            ViewData["TenantId"] = new SelectList(_context.Tenant, "Id", "FirstName", rentedCars.TenantId);
            return View(rentedCars);
        }

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
