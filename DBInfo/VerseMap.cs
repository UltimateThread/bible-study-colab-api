using BibleStudyColabApi.Models;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibleStudyColabApi
{
   public class VerseMap : ClassMap<Verse>
   {
      public VerseMap()
      {
         Map(m => m.Id).Name("Id");
         Map(m => m.BibleVersionId).Name("BibleVersionId");
         Map(m => m.Book).Name("Book");
         Map(m => m.Testament).Name("Testament");
         Map(m => m.ChapterNumber).Name("ChapterNumber");
         Map(m => m.VerseNumber).Name("VerseNumber");
         Map(m => m.Text).Name("Text");
      }
   }
}
