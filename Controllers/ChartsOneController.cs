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
    public class ChartsOneController : ControllerBase
    {
        private readonly SimsovisionDBContext _context;
        public ChartsOneController(SimsovisionDBContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var types = _context.ParticipantTypes.Include(p => p.Participants).ToList();
            List<object> typeParts = new List<object>();
            typeParts.Add(new[] { "ParticipantTypes", "Participants" });
            foreach (var p in types)
            {
                typeParts.Add(new object[] { p.ParticipantType, p.Participants.Count() });
            }
            return new JsonResult(typeParts);
        }
    }
}