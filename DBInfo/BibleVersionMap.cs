using BibleStudyColabApi.Models;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibleStudyColabApi
{
   public class BibleVersionMap : ClassMap<BibleVersion>
   {
      public BibleVersionMap()
      {
         Map(m => m.Id).Name("Id");
         Map(m => m.Abbreviation).Name("Abbreviation");
         Map(m => m.Language).Name("Language");
         Map(m => m.Version).Name("Version");
         Map(m => m.InfoUrl).Name("InfoUrl");
      }
   }
}
