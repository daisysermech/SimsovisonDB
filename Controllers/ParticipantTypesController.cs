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
        [HttpPost]
        public string ReturnTypes(string type)
        {
            return type;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                ParticipantTypes newtype;
                                var t = (from type in _context.ParticipantTypes where type.ParticipantType.Contains(worksheet.Name) select type).ToList();
                                if (t.Count > 0)
                                    newtype = t[0];

                                else
                                {
                                    newtype = new ParticipantTypes();
                                    newtype.ParticipantType = worksheet.Name;
                                    _context.ParticipantTypes.Add(newtype);
                                }
                                //перегляд усіх рядків                    
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        //участник
                                        Participants participant = new Participants();
                                        var p = (from part in _context.Participants where part.ParticipantName== row.Cell(1).Value.ToString() select part).ToList();
                                        if (p.Count > 0)
                                            participant = p[0];
                                        else
                                        {
                                            participant.ParticipantName = row.Cell(1).Value.ToString();
                                            participant.ParticipantDate = Convert.ToDateTime(row.Cell(2).Value.ToString());
                                            participant.Biography = row.Cell(3).Value.ToString();

                                            //участник -> город
                                            Cities newcity;
                                            var c = (from city in _context.Cities where city.CityName.Contains(row.Cell(4).Value.ToString()) select city).ToList();
                                            if (c.Count > 0)
                                                newcity = c[0];
                                            else
                                            {
                                                newcity = new Cities();
                                                newcity.CityName = row.Cell(4).Value.ToString();
                                                newcity.Description = "Imported from file.";
                                                _context.Cities.Add(newcity);
                                            }

                                            participant.IdRepresentedCityNavigation = newcity;
                                            participant.IdParticipantTypeNavigation = newtype;
                                            _context.Participants.Add(participant);
                                        }

                                        //участие
                                        Participations participation = new Participations();
                                        var pt = (from parts in _context.Participations where parts.IdParticipantNavigation.ParticipantName == row.Cell(1).Value.ToString() select parts).ToList();
                                        if (pt.Count > 0)
                                            participation = pt[0];
                                        else
                                        {

                                            //песня
                                            Songs song = new Songs();
                                            var s = (from songs in _context.Songs where song.SongName == row.Cell(5).Value.ToString() select songs).ToList();
                                            if (s.Count > 0)
                                                song = s[0];
                                            else
                                            {
                                                song.SongName = row.Cell(5).Value.ToString();
                                                string myString = row.Cell(6).Value.ToString();
                                                double d = double.Parse(myString);
                                                DateTime dt = DateTime.FromOADate(d);
                                                song.Duration = new TimeSpan(dt.Hour, dt.Minute, dt.Second);
                                                _context.Songs.Add(song);
                                            }

                                            //участие -> год
                                            Years newyear;
                                            var y = (from year in _context.Years where year.YearOfContest == int.Parse(row.Cell(7).Value.ToString()) select year).ToList();
                                            if (y.Count > 0)
                                                newyear = y[0];
                                            else
                                            {
                                                newyear = new Years();
                                                newyear.YearOfContest = Convert.ToInt32(row.Cell(7).Value);
                                                newyear.Slogan = "Slogan";
                                                newyear.Stage = "Stage";
                                                foreach (var city in _context.Cities)
                                                    if (city.CityName == "POLIGON")
                                                    {
                                                        newyear.IdHostCityNavigation = city;
                                                        break;
                                                    }
                                                _context.Years.Add(newyear);
                                            }

                                            //участие -> номинация
                                            Nominations newnomination;
                                            var n = (from nomination in _context.Nominations where nomination.NominationName.Contains(row.Cell(8).Value.ToString()) select nomination).ToList();
                                            if (n.Count > 0)
                                                newnomination = n[0];
                                            else
                                            {
                                                newnomination = new Nominations();
                                                newnomination.NominationName = row.Cell(8).Value.ToString();
                                                _context.Nominations.Add(newnomination);
                                            }


                                            participation.IdParticipantNavigation = participant;
                                            participation.IdSongNavigation = song;
                                            participation.IdYearOfContestNavigation = newyear;
                                            participation.IdNominationNavigation = newnomination;
                                            participation.Place = Convert.ToInt32(row.Cell(9).Value);

                                            _context.Participations.Add(participation);
                                        }
                                    }
                                    catch
                                    {
                                        RedirectToAction("ErrorOpen", "Home");
                                    }
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var part_types = _context.ParticipantTypes.Include("Participants").ToList();

                foreach (var p in part_types)
                {
                    var worksheet = workbook.Worksheets.Add(p.ParticipantType);
                    worksheet.Cell("A1").Value = "Имя участника";
                    worksheet.Cell("B1").Value = "Дата рождения/создания";
                    worksheet.Cell("C1").Value = "Биография";
                    worksheet.Cell("D1").Value = "Город";
                    worksheet.Cell("E1").Value = "Песня";
                    worksheet.Cell("F1").Value = "Длительность";
                    worksheet.Cell("G1").Value = "Год участия";
                    worksheet.Cell("H1").Value = "Номинация";
                    worksheet.Cell("I1").Value = "Место";
                    worksheet.Row(1).Style.Font.Bold = true;
                    var participants = p.Participants.ToList();

                    for (int i = 0; i < participants.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = participants[i].ParticipantName;
                        worksheet.Cell(i + 2, 2).Value = participants[i].ParticipantDate;
                        worksheet.Cell(i + 2, 3).Value = participants[i].Biography;
                        var cities = (from city in _context.Cities where city.IdCity == participants[i].IdRepresentedCity select city).ToList();
                        worksheet.Cell(i + 2, 4).Value = cities[0].CityName;


                        var participations = (from parts in _context.Participations where parts.IdParticipant == participants[i].IdParticipant select parts).ToList();

                        var songs = (from song in _context.Songs where participations[0].IdSong == song.IdSong select song).ToList();
                        var years = (from year in _context.Years where participations[0].IdYearOfContest == year.IdYearOfContest select year).ToList();
                        var nominations = (from nomination in _context.Nominations where nomination.IdNomination == participations[0].IdNomination select nomination).ToList();

                        worksheet.Cell(i + 2, 5).Value = songs[0].SongName;
                        worksheet.Cell(i + 2, 6).Value = songs[0].Duration;

                        worksheet.Cell(i + 2, 7).Value = years[0].YearOfContest;

                        worksheet.Cell(i + 2, 8).Value = nominations[0].NominationName;

                        worksheet.Cell(i + 2, 9).Value = participations[0].Place;
                    }
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"library_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }
    }
}
