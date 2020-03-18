using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SimsovisionDataBase;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Http;
using ClosedXML.Excel;

namespace SimsovisionDataBase.Controllers
{
    [Authorize(Roles = "admin,moder,user")]
    public class ParticipantTypesController : Controller
    {
        private readonly SimsovisionDBContext _context;

        public ParticipantTypesController(SimsovisionDBContext context)
        {
            _context = context;
        }

        // GET: ParticipantTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ParticipantTypes.ToListAsync());
        }

        // GET: ParticipantTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participantTypes = await _context.ParticipantTypes
                .FirstOrDefaultAsync(m => m.IdParticipantType == id);
            if (participantTypes == null)
            {
                return NotFound();
            }

            //return View(participantTypes);
            return RedirectToAction("Index", "Participants", new { id = participantTypes.IdParticipantType, type = participantTypes.ParticipantType });
        }

        // GET: ParticipantTypes/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ParticipantTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("IdParticipantType,ParticipantType")] ParticipantTypes participantTypes)
        {
            //if (!User.IsInRole("admin")) RedirectToAction("Denied", "Account");
            if (ModelState.IsValid)
            {
                _context.Add(participantTypes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(participantTypes);
        }

        // GET: ParticipantTypes/Edit/5
        [Authorize(Roles = "admin,moder")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participantTypes = await _context.ParticipantTypes.FindAsync(id);
            if (participantTypes == null)
            {
                return NotFound();
            }
            return View(participantTypes);
        }

        // POST: ParticipantTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,moder")]
        public async Task<IActionResult> Edit(int id, [Bind("IdParticipantType,ParticipantType")] ParticipantTypes participantTypes)
        {
            if (id != participantTypes.IdParticipantType)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(participantTypes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParticipantTypesExists(participantTypes.IdParticipantType))
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
            return View(participantTypes);
        }

        // GET: ParticipantTypes/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participantTypes = await _context.ParticipantTypes
                .FirstOrDefaultAsync(m => m.IdParticipantType == id);
            if (participantTypes == null)
            {
                return NotFound();
            }

            return View(participantTypes);
        }

        // POST: ParticipantTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var participantTypes = await _context.ParticipantTypes.FindAsync(id);
                _context.ParticipantTypes.Remove(participantTypes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("DeleteErr", "Home");
            }
        }

        private bool ParticipantTypesExists(int id)
        {
            return _context.ParticipantTypes.Any(e => e.IdParticipantType == id);
        }
    }
}
