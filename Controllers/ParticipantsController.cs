using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SimsovisionDataBase;
using Microsoft.AspNetCore.Authorization;
using ClosedXML.Excel;
using System.IO;

namespace SimsovisionDataBase.Controllers
{
    [Authorize(Roles = "admin, moder, user")]
    public class ParticipantsController : Controller
    {
        private readonly SimsovisionDBContext _context;

        public ParticipantsController(SimsovisionDBContext context)
        {
            _context = context;
        }

        // GET: Participants
        public async Task<IActionResult> Index(int? id, string? type)
        {
            if (id == null)
            {
                var simsovisionDBContext = _context.Participants.Include(p => p.IdParticipantTypeNavigation).Include(p => p.IdRepresentedCityNavigation);
                return View(await simsovisionDBContext.ToListAsync());
            }
            //ViewBag.IdParticipantType = id;
            //ViewBag.ParticipantType = type;
            var simsovisionDBContext1 = _context.Participants.Where(p => p.IdParticipantType == id).Include(p => p.IdParticipantTypeNavigation).Include(p=>p.IdRepresentedCityNavigation);
            return View(await simsovisionDBContext1.ToListAsync());
        }

        // GET: Participants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participants = await _context.Participants
                .Include(p => p.IdParticipantTypeNavigation)
                .Include(p => p.IdRepresentedCityNavigation)
                .FirstOrDefaultAsync(m => m.IdParticipant == id);
            if (participants == null)
            {
                return NotFound();
            }

            //return View(participants);
            return RedirectToAction("Index","Participations", new { part_id = participants.IdParticipant, part_name = participants.ParticipantName});
        }

        // GET: Participants/Create
        [Authorize(Roles = "admin, moder")]
        public IActionResult Create()
        {
            ViewData["IdParticipantType"] = new SelectList(_context.ParticipantTypes, "IdParticipantType", "ParticipantType");
            ViewData["IdRepresentedCity"] = new SelectList(_context.Cities.OrderBy(c=>c.CityName), "IdCity", "CityName");
            return View();
        }

        // POST: Participants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, moder")]
        public async Task<IActionResult> Create([Bind("IdParticipant,ParticipantName,IdRepresentedCity,IdParticipantType,ParticipantDate,Biography")] Participants participants)
        {
            if (ModelState.IsValid)
            {
                _context.Add(participants);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdParticipantType"] = new SelectList(_context.ParticipantTypes, "IdParticipantType", "ParticipantType", participants.IdParticipantType);
            ViewData["IdRepresentedCity"] = new SelectList(_context.Cities, "IdCity", "CityName", participants.IdRepresentedCity);
            return View(participants);
        }

        // GET: Participants/Edit/5
        [Authorize(Roles = "admin, moder")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participants = await _context.Participants.FindAsync(id);
            if (participants == null)
            {
                return NotFound();
            }
            ViewData["IdParticipantType"] = new SelectList(_context.ParticipantTypes, "IdParticipantType", "ParticipantType", participants.IdParticipantType);
            ViewData["IdRepresentedCity"] = new SelectList(_context.Cities, "IdCity", "CityName", participants.IdRepresentedCity);
            return View(participants);
        }

        // POST: Participants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, moder")]
        public async Task<IActionResult> Edit(int id, [Bind("IdParticipant,ParticipantName,IdRepresentedCity,IdParticipantType,ParticipantDate,Biography")] Participants participants)
        {
            if (id != participants.IdParticipant)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(participants);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParticipantsExists(participants.IdParticipant))
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
            ViewData["IdParticipantType"] = new SelectList(_context.ParticipantTypes, "IdParticipantType", "ParticipantType", participants.IdParticipantType);
            ViewData["IdRepresentedCity"] = new SelectList(_context.Cities, "IdCity", "CityName", participants.IdRepresentedCity);
            return View(participants);
        }

        // GET: Participants/Delete/5
        [Authorize(Roles = "admin, moder")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participants = await _context.Participants
                .Include(p => p.IdParticipantTypeNavigation)
                .Include(p => p.IdRepresentedCityNavigation)
                .FirstOrDefaultAsync(m => m.IdParticipant == id);
            if (participants == null)
            {
                return NotFound();
            }

            return View(participants);
        }

        // POST: Participants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, moder")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var participants = await _context.Participants.FindAsync(id);
                _context.Participants.Remove(participants);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("DeleteErr", "Home");
            }
        }

        private bool ParticipantsExists(int id)
        {
            return _context.Participants.Any(e => e.IdParticipant == id);
        }

        [HttpGet]
        public JsonResult CheckDate(DateTime date)
        {
            var result = !((date.Year <= DateTime.MinValue.Year) || (date.Year >= DateTime.Now.Year));
            return Json(result);
        }
    }
}
