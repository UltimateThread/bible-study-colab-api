using BibleStudyColabApi.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibleStudyColabApi.Controllers
{
   public class BibleVersionsController : ODataController
   {
      private readonly BibleContext _context;

      public BibleVersionsController(BibleContext context)
      {
         _context = context;
      }

      [EnableQuery]
      public IActionResult Get()
      {
         return Ok(_context.BibleVersions);
      }

      [EnableQuery]
      public IActionResult Get(string key)
      {
         return Ok(_context.BibleVersions.FirstOrDefault(c => c.Id == key));
      }
   }
}
