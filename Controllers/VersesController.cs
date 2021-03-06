﻿using BibleStudyColabApi.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibleStudyColabApi.Controllers
{
   public class VersesController : ODataController
   {
      private readonly BibleContext _context;

      public VersesController(BibleContext context)
      {
         _context = context;
      }

      [EnableQuery]
      public IActionResult Get()
      {
         return Ok(_context.Verses);
      }

      [EnableQuery]
      public IActionResult Get(Guid key)
      {
         return Ok(_context.Verses.FirstOrDefault(c => c.Id == key));
      }
   }
}
