using CarRentingProject_Melvin.Areas.Identity.Data;
using CarRentingProject_Melvin.Data;
using CarRentingProject_Melvin.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentingProject_Melvin.Controllers
{
    public class AppLangController : AppController
    {
        public AppLangController(DBContext context, IHttpContextAccessor httpContextAccessor,
                                 ILogger<AppController> logger) :
                                 base(context, httpContextAccessor, logger)
        {
        }


        public IActionResult ChangeLanguage(string id, string returnUrl)
        {
            string c = Thread.CurrentThread.CurrentCulture.ToString();
            c = id + c.Substring(2);

            if (c.Length != 5) c = id;

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(c)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            if (_user.Id != "-")
            {
                _user.AppLangId = id;
                Language lang = _context.Languages.FirstOrDefault(l => l.AppLangId == id);
                _user.Language = lang;
                CarRentingProject_AppUser user = _context.Users.FirstOrDefault(u => u.Id == _user.Id);
                user.Language = lang;
                _context.SaveChanges();
            }

            return LocalRedirect(returnUrl);
        }


        // GET: Languages
        public async Task<IActionResult> Index()
        {
            return View(await _context.Languages.ToListAsync());
        }

        // GET: Languages/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var language = await _context.Languages
                .FirstOrDefaultAsync(m => m.AppLangId == id);
            if (language == null)
            {
                return NotFound();
            }

            return View(language);
        }

        // GET: Languages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Languages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Cultures,IsSystemLanguage")] Language language)
        {
            if (ModelState.IsValid)
            {
                _context.Add(language);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(language);
        }

        // GET: Languages/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var language = await _context.Languages.FindAsync(id);
            if (language == null)
            {
                return NotFound();
            }
            return View(language);
        }

        // POST: Languages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Cultures,IsSystemLanguage")] Language lang)
        {
            if (id != lang.AppLangId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LanguageExists(lang.AppLangId))
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
            return View(lang);
        }

        // GET: Languages/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lang = await _context.Languages
                .FirstOrDefaultAsync(m => m.AppLangId == id);
            if (lang == null)
            {
                return NotFound();
            }

            return View(lang);
        }

        // POST: Languages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var lang = await _context.Languages.FindAsync(id);
            _context.Languages.Remove(lang);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LanguageExists(string id)
        {
            return _context.Languages.Any(e => e.AppLangId == id);
        }
    }
}
