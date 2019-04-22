using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibleStudyColabApi.Models
{
   public class Verse
   {
      public Guid Id { get; set; }
      public string BibleVersionId { get; set; }
      public BibleVersion BibleVersion { get; set; }
      public string Book { get; set; }
      public string Testament { get; set; }
      public int ChapterNumber { get; set; }
      public int VerseNumber { get; set; }
      public string Text { get; set; }
   }
}
