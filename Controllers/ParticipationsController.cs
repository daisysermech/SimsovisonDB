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
    public class ParticipationsController : Controller
    {
        private readonly SimsovisionDBContext _context;

        public ParticipationsController(SimsovisionDBContext context)
        {
            _context = context;
        }

        // GET: Participations
        public async Task<IActionResult> Index(int? part_id, int? nom_id, int? song_id, int? year_id,int? city_id)
        {
            if (part_id != null)
            {
                //ViewBag.IdParticipant = part_id;
                //ViewBag.Participant = part_name;
                var simsovisionDBContext1 = _context.Participations.Where(p => p.IdParticipant == part_id).Include(p => p.IdNominationNavigation).Include(p => p.IdParticipantNavigation).Include(p => p.IdSongNavigation).Include(p => p.IdYearOfContestNavigation);
                return View(await simsovisionDBContext1.ToListAsync());
            }
            if (nom_id != null)
            {
                //ViewBag.IdNomination = nom_id;
                //ViewBag.Nomination = nom_name;
                var simsovisionDBContext1 = _context.Participations.Where(p => p.IdNomination == nom_id).Include(p => p.IdNominationNavigation).Include(p => p.IdParticipantNavigation).Include(p => p.IdSongNavigation).Include(p => p.IdYearOfContestNavigation);
                return View(await simsovisionDBContext1.ToListAsync());
            }
            if (song_id != null)
            {
                //ViewBag.IdNomination = nom_id;
                //ViewBag.Nomination = nom_name;
                var simsovisionDBContext1 = _context.Participations.Where(p => p.IdSong == song_id).Include(p => p.IdNominationNavigation).Include(p => p.IdParticipantNavigation).Include(p => p.IdSongNavigation).Include(p => p.IdYearOfContestNavigation);
                return View(await simsovisionDBContext1.ToListAsync());
            }
            if (year_id != null)
            {
                //ViewBag.IdNomination = nom_id;
                //ViewBag.Nomination = nom_name;
                var simsovisionDBContext1 = _context.Participations.Where(p => p.IdYearOfContest == year_id).Include(p => p.IdNominationNavigation).Include(p => p.IdParticipantNavigation).Include(p => p.IdSongNavigation).Include(p => p.IdYearOfContestNavigation);
                return View(await simsovisionDBContext1.ToListAsync());
            }
            /*if (city_id != null)
            {
                //ViewBag.IdNomination = nom_id;
                //ViewBag.Nomination = nom_name;
                var simsovisionDBContext1 = _context.Participations.Where(p => p.IdParticipant == city_id).Include(p => p.IdNominationNavigation).Include(p => p.IdParticipantNavigation).Include(p => p.IdSongNavigation).Include(p => p.IdYearOfContestNavigation);
                return View(await simsovisionDBContext1.ToListAsync());
            }*/

            var simsovisionDBContext = _context.Participations.Include(p => p.IdNominationNavigation).Include(p => p.IdParticipantNavigation).Include(p => p.IdSongNavigation).Include(p => p.IdYearOfContestNavigation);
            return View(await simsovisionDBContext.ToListAsync());
        }

        // GET: Participations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participations = await _context.Participations
                .Include(p => p.IdNominationNavigation)
                .Include(p => p.IdParticipantNavigation)
                .Include(p => p.IdSongNavigation)
                .Include(p => p.IdYearOfContestNavigation)
                .FirstOrDefaultAsync(m => m.IdParticipation == id);
            if (participations == null)
            {
                return NotFound();
            }

            return View(participations);
        }

        // GET: Participations/Create
        [Authorize(Roles = "admin, moder")]
        public IActionResult Create()
        {
            ViewData["IdNomination"] = new SelectList(_context.Nominations, "IdNomination", "NominationName");
            ViewData["IdParticipant"] = new SelectList(_context.Participants, "IdParticipant", "ParticipantName");
            ViewData["IdSong"] = new SelectList(_context.Songs, "IdSong", "SongName");
            ViewData["IdYearOfContest"] = new SelectList(_context.Years, "IdYearOfContest", "YearOfContest");
            return View();
        }

        // POST: Participations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, moder")]
        public async Task<IActionResult> Create([Bind("IdParticipation,IdYearOfContest,IdParticipant,IdSong,IdNomination,Place")] Participations participations)
        {
            if (ModelState.IsValid)
            {
                _context.Add(participations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdParticipant"] = new SelectList(_context.Participants, "IdParticipant", "ParticipantName", participations.IdParticipant);
            ViewData["IdSong"] = new SelectList(_context.Songs, "IdSong", "SongName", participations.IdSong);
            ViewData["IdYearOfContest"] = new SelectList(_context.Years, "IdYearOfContest", "YearOfContest", participations.IdYearOfContest);
            ViewData["IdNomination"] = new SelectList(_context.Nominations, "IdNomination", "NominationName", participations.IdNomination);
            return View(participations);
        }

        // GET: Participations/Edit/5
        [Authorize(Roles = "admin, moder")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participations = await _context.Participations.FindAsync(id);
            if (participations == null)
            {
                return NotFound();
            }
            ViewData["IdNomination"] = new SelectList(_context.Nominations, "IdNomination", "NominationName", participations.IdNomination);
            ViewData["IdParticipant"] = new SelectList(_context.Participants, "IdParticipant", "ParticipantName", participations.IdParticipant);
            ViewData["IdSong"] = new SelectList(_context.Songs, "IdSong", "SongName", participations.IdSong);
            ViewData["IdYearOfContest"] = new SelectList(_context.Years, "IdYearOfContest", "YearOfContest", participations.IdYearOfContest);
            return View(participations);
        }

        // POST: Participations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, moder")]
        public async Task<IActionResult> Edit(int id, [Bind("IdParticipation,IdYearOfContest,IdParticipant,IdSong,IdNomination,Place")] Participations participations)
        {
            if (id != participations.IdParticipation)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(participations);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParticipationsExists(participations.IdParticipation))
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
            ViewData["IdNomination"] = new SelectList(_context.Nominations, "IdNomination", "NominationName", participations.IdNomination);
            ViewData["IdParticipant"] = new SelectList(_context.Participants, "IdParticipant", "ParticipantName", participations.IdParticipant);
            ViewData["IdSong"] = new SelectList(_context.Songs, "IdSong", "SongName", participations.IdSong);
            ViewData["IdYearOfContest"] = new SelectList(_context.Years, "IdYearOfContest", "YearOfContest", participations.IdYearOfContest);
            return View(participations);
        }

        // GET: Participations/Delete/5
        [Authorize(Roles = "admin, moder")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participations = await _context.Participations
                .Include(p => p.IdNominationNavigation)
                .Include(p => p.IdParticipantNavigation)
                .Include(p => p.IdSongNavigation)
                .Include(p => p.IdYearOfContestNavigation)
                .FirstOrDefaultAsync(m => m.IdParticipation == id);
            if (participations == null)
            {
                return NotFound();
            }

            return View(participations);
        }

        // POST: Participations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, moder")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var participations = await _context.Participations.FindAsync(id);
                _context.Participations.Remove(participations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("DeleteErr", "Home");
            }
        }

        private bool ParticipationsExists(int id)
        {
            return _context.Participations.Any(e => e.IdParticipation == id);
        }
    }
}
