using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BibleStudyColabApi.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BibleStudyColabApi
{
   public class Program
   {
      public static void Main(string[] args)
      {
         // Due to the following error in building, we need to manually populate the DB
         // error CS8103: Combined length of user strings used by the program exceeds allowed limit. Try to decrease use of string literals.
         // This error is caused by the initial migration being too long
         // To fix this the verses are extracted out into txt files and the DB is populated from those txt files

         /*
          To create and populate the DB do the following:
            1. Open Package Manager Console and run Update-Database (This will create the Bible database)
            2. When you first run the application, the Bible database will be populated if no verses are found
          */
         PopulateDB();

         CreateWebHostBuilder(args).Build().Run();
      }

      private static void PopulateDB()
      {
         var connectionString = @"Server=(localdb)\mssqllocaldb;Database=Bible;Trusted_Connection=True;ConnectRetryCount=0";
         var optionsBuilder = new DbContextOptionsBuilder<BibleContext>();
         optionsBuilder.UseSqlServer(connectionString);
         BibleContext dbContext = new BibleContext(optionsBuilder.Options);

         if (dbContext.BibleVersions.Count() == 0)
         {
            dbContext.BibleVersions.Add(new BibleVersion { Id = "1000001", Abbreviation = "ASV", Language = "english", Version = "American Standard - ASV1901", InfoUrl = "http://en.wikipedia.org/wiki/American_Standard_Version" });
            dbContext.BibleVersions.Add(new BibleVersion { Id = "1000002", Abbreviation = "BBE", Language = "english", Version = "Bible in Basic English", InfoUrl = "http://en.wikipedia.org/wiki/Bible_in_Basic_English" });
            dbContext.BibleVersions.Add(new BibleVersion { Id = "1000003", Abbreviation = "DARBY", Language = "english", Version = "Darby English Bible", InfoUrl = "http://en.wikipedia.org/wiki/Darby_Bible" });
            dbContext.BibleVersions.Add(new BibleVersion { Id = "1000004", Abbreviation = "KJV", Language = "english", Version = "King James Version", InfoUrl = "http://en.wikipedia.org/wiki/King_James_Version" });
            dbContext.BibleVersions.Add(new BibleVersion { Id = "1000005", Abbreviation = "WBT", Language = "english", Version = "Webster's Bible", InfoUrl = "http://en.wikipedia.org/wiki/Webster%27s_Revision" });
            dbContext.BibleVersions.Add(new BibleVersion { Id = "1000006", Abbreviation = "WEB", Language = "english", Version = "World English Bible", InfoUrl = "http://en.wikipedia.org/wiki/World_English_Bible" });
            dbContext.BibleVersions.Add(new BibleVersion { Id = "1000007", Abbreviation = "YLT", Language = "english", Version = "Young's Literal Translation", InfoUrl = "http://en.wikipedia.org/wiki/Young%27s_Literal_Translation" });
            dbContext.SaveChanges();
         }

         if (dbContext.Verses.Count() == 0)
         {
            string[] files = Directory.GetFiles(@"Verses");

            List<Task> tasks = new List<Task>();
            foreach (var file in files)
            {
               object arg = file;
               var task = new TaskFactory().StartNew(new Action<object>((test) =>
               {
                  Debug.WriteLine($"Adding Verses from File: {test}");

                  foreach (var line in File.ReadAllLines(test.ToString()))
                  {
                     var values = line.Split("%%");

                     if (values.Length < 6)
                        throw new Exception("Not enough values");

                     BibleContext bibleContext = new BibleContext(optionsBuilder.Options);

                     var bibleVersionId = values[0];
                     var book = values[1];
                     var testament = values[2];
                     var chapterNumber = values[3];
                     var verseNumber = values[4];
                     var text = values[5];

                     var verse = new Verse()
                     {
                        Id = Guid.NewGuid(),
                        BibleVersionId = bibleVersionId,
                        Book = book,
                        Testament = testament,
                        ChapterNumber = Convert.ToInt32(chapterNumber),
                        VerseNumber = Convert.ToInt32(verseNumber),
                        Text = text
                     };

                     Debug.WriteLine($"Adding Verse: BibleVersionId={verse.BibleVersionId}, Book={verse.Book}, Chapter={verse.ChapterNumber}, Verse={verse.VerseNumber}");
                     bibleContext.Verses.Add(verse);
                     bibleContext.SaveChanges();
                  }
               }), arg);

               tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
            Debug.WriteLine("Finished Populating DB");
         }
      }

      public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
          WebHost.CreateDefaultBuilder(args)
              .UseStartup<Startup>();
   }
}
