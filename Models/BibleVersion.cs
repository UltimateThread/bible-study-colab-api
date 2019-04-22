using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibleStudyColabApi.Models
{
   public class BibleVersion
   {
      public string Id { get; set; }
      public string Abbreviation { get; set; }
      public string Language { get; set; }
      public string Version { get; set; }
      public string InfoUrl { get; set; }
      public List<Verse> Verses { get; set; }
   }
}
