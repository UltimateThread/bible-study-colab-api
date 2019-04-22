using BibleStudyColabApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibleStudyColabApi.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class VersesController : ControllerBase
   {
      private readonly BibleContext _context;

      public VersesController(BibleContext context)
      {
         _context = context;
      }

      // GET api/values
      [HttpGet]
      public ActionResult<IEnumerable<string>> Get()
      {
         var test = _context.BibleVersions.Include(x => x.Verses).ToList();


         return new string[] { "value1", "value2" };
      }
   }
}
