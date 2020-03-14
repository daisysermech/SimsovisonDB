using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SimsovisionDataBase;
using Microsoft.AspNetCore.Authorization;

namespace SimsovisionDataBase.Controllers
{
    [Authorize(Roles = "admin, moder, user")]
    public class YearsController : Controller
    {
        private readonly SimsovisionDBContext _context;

        public YearsController(SimsovisionDBContext context)
        {
            _context = context;
        }

        // GET: Years
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                var simsovisionDBContext = _context.Years.Include(y => y.IdHostCityNavigation);
                return View(await simsovisionDBContext.ToListAsync());
            }

            //ViewBag.IdParticipantType = id;
            //ViewBag.ParticipantType = type;
            var simsovisionDBContext1 = _context.Years.Where(y => y.IdHostCity == id).Include(y => y.IdHostCityNavigation);
            return View(await simsovisionDBContext1.ToListAsync());
        }

        // GET: Years/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var years = await _context.Years
                .Include(y => y.IdHostCityNavigation)
                .FirstOrDefaultAsync(m => m.IdYearOfContest == id);
            if (years == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Participations", new { year_id = years.IdYearOfContest });
            //return View(years);
        }

        // GET: Years/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewData["IdHostCity"] = new SelectList(_context.Cities.OrderBy(c => c.CityName), "IdCity", "CityName");
            return View();
        }

        // POST: Years/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("IdYearOfContest,YearOfContest,IdHostCity,Slogan,Stage")] Years years)
        {
            if (ModelState.IsValid)
            {
                _context.Add(years);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdHostCity"] = new SelectList(_context.Cities, "IdCity", "CityName", years.IdHostCity);
            return View(years);
        }

        // GET: Years/Edit/5
        [Authorize(Roles = "admin, moder")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var years = await _context.Years.FindAsync(id);
            if (years == null)
            {
                return NotFound();
            }
            ViewData["IdHostCity"] = new SelectList(_context.Cities, "IdCity", "CityName", years.IdHostCity);
            return View(years);
        }

        // POST: Years/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, moder")]
        public async Task<IActionResult> Edit(int id, [Bind("IdYearOfContest,YearOfContest,IdHostCity,Slogan,Stage")] Years years)
        {
            if (id != years.IdYearOfContest)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(years);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!YearsExists(years.IdYearOfContest))
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
            ViewData["IdHostCity"] = new SelectList(_context.Cities, "IdCity", "CityName", years.IdHostCity);
            return View(years);
        }

        // GET: Years/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var years = await _context.Years
                .Include(y => y.IdHostCityNavigation)
                .FirstOrDefaultAsync(m => m.IdYearOfContest == id);
            if (years == null)
            {
                return NotFound();
            }

            return View(years);
        }

        // POST: Years/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var years = await _context.Years.FindAsync(id);
                _context.Years.Remove(years);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("DeleteErr", "Home");
            }
        }

        private bool YearsExists(int id)
        {
            return _context.Years.Any(e => e.IdYearOfContest == id);
        }
    }
}
