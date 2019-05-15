using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BibleStudyColabApi.Models;
using CsvHelper;
using CsvHelper.Configuration;
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
         // To fix this the verses are extracted out into CSV files and the DB is populated from those CSV files

         /*
          To create and populate the DB do the following:
            1. Open Package Manager Console and run Update-Database (This will create the Bible database)
            2. When you first run the application, the Bible database will be populated if no verses are found
         */
         try
         {
            PopulateDB();
         }
         catch (Exception ex)
         {
            Console.WriteLine($"Failed to Populate DB: {ex.Message}");
         }

         CreateWebHostBuilder(args).Build().Run();
      }

      private static void PopulateDB()
      {
         var connectionString = @"Server=(localdb)\mssqllocaldb;Database=Bible;Trusted_Connection=True;ConnectRetryCount=0";
         var optionsBuilder = new DbContextOptionsBuilder<BibleContext>();
         optionsBuilder.UseSqlServer(connectionString);
         using (var dbContext = new BibleContext(optionsBuilder.Options))
         {
            if (dbContext.BibleVersions.Count() == 0)
            {
               // Find the BibleVersions.csv file and load its data into a collection of BibleVersion objects.
               // Use those objects to populate the DB.
               var file = Directory.GetFiles(@"DBInfo").Where(x => Path.GetFileName(x).Equals("BibleVersions.csv", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
               if (file != null)
               {
                  using (var reader = new StreamReader(file))
                  using (var csv = new CsvReader(reader))
                  using (var bibleContext = new BibleContext(optionsBuilder.Options))
                  {
                     csv.Configuration.RegisterClassMap<BibleVersionMap>();
                     var records = csv.GetRecords<BibleVersion>().ToList();

                     bibleContext.BibleVersions.AddRange(records);
                     bibleContext.SaveChanges();
                  }
               }
            }

            if (dbContext.Verses.Count() == 0)
            {
               // Find all the Verses CSV files and load their data into a collection of Verse objects.
               // User those objects to populate the DB.
               var files = Directory.GetFiles(@"DBInfo").Where(x => Path.GetExtension(x).Equals(".csv", StringComparison.CurrentCultureIgnoreCase) &&
               !x.Contains("BibleVersions.csv")).ToList();

               foreach (var file in files)
               {
                  Debug.WriteLine($"Adding Verses from File: {file}");

                  using (var reader = new StreamReader(file))
                  using (var csv = new CsvReader(reader))
                  using (var bibleContext = new BibleContext(optionsBuilder.Options))
                  {
                     csv.Configuration.RegisterClassMap<VerseMap>();
                     var records = csv.GetRecords<Verse>().ToList();

                     foreach (var record in records)
                        bibleContext.Verses.Add(record);

                     bibleContext.SaveChanges();
                  }
               }
               Debug.WriteLine("Finished Populating DB");
            }
         }
      }

      public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
         WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();
   }
}
