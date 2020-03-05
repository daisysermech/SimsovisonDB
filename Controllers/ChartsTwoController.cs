using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SimsovisionDataBase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsTwoController : ControllerBase
    {
        private readonly SimsovisionDBContext _context;
        public ChartsTwoController(SimsovisionDBContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var years = _context.Years.Include(p => p.Participations).ToList();
            List<object> yearlist = new List<object>();
            yearlist.Add(new[] { "Year", "Participants" });
            foreach (var p in years)
            {
                yearlist.Add(new object[] { p.YearOfContest.ToString(), p.Participations.Count() });
            }
            return new JsonResult(yearlist);
        }
    }
}