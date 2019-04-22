using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BibleStudyColabApi.Models
{
   public class BibleContext : DbContext
   {
      public BibleContext(DbContextOptions<BibleContext> options)
          : base(options)
      { }

      public DbSet<BibleVersion> BibleVersions { get; set; }
      public DbSet<Verse> Verses { get; set; }
   }
}