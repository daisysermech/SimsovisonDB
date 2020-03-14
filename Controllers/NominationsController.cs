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
    public class NominationsController : Controller
    {
        private readonly SimsovisionDBContext _context;

        public NominationsController(SimsovisionDBContext context)
        {
            _context = context;
        }

        // GET: Nominations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Nominations.ToListAsync());
        }

        // GET: Nominations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nominations = await _context.Nominations
                .FirstOrDefaultAsync(m => m.IdNomination == id);
            if (nominations == null)
            {
                return NotFound();
            }
            return RedirectToAction("Index", "Participations", new { nom_id = nominations.IdNomination});

            //return View(nominations);
        }

        // GET: Nominations/Create
        [Authorize(Roles = "admin, moder")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Nominations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, moder")]
        public async Task<IActionResult> Create([Bind("IdNomination,NominationName")] Nominations nominations)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nominations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nominations);
        }

        // GET: Nominations/Edit/5
        [Authorize(Roles = "admin, moder")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nominations = await _context.Nominations.FindAsync(id);
            if (nominations == null)
            {
                return NotFound();
            }
            return View(nominations);
        }

        // POST: Nominations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, moder")]
        public async Task<IActionResult> Edit(int id, [Bind("IdNomination,NominationName")] Nominations nominations)
        {
            if (id != nominations.IdNomination)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nominations);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NominationsExists(nominations.IdNomination))
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
            return View(nominations);
        }

        // GET: Nominations/Delete/5
        [Authorize(Roles = "admin, moder")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nominations = await _context.Nominations
                .FirstOrDefaultAsync(m => m.IdNomination == id);
            if (nominations == null)
            {
                return NotFound();
            }

            return View(nominations);
        }

        // POST: Nominations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, moder")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var nominations = await _context.Nominations.FindAsync(id);
                _context.Nominations.Remove(nominations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("DeleteErr", "Home");
            }
        }

        private bool NominationsExists(int id)
        {
            return _context.Nominations.Any(e => e.IdNomination == id);
        }
    }
}
