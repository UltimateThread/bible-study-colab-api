﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibleStudyColabApi.Helpers;
using BibleStudyColabApi.Models;
using BibleStudyColabApi.Services;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;

namespace BibleStudyColabApi
{
   public class Startup
   {
      public Startup(IConfiguration configuration)
      {
         Configuration = configuration;
      }

      public IConfiguration Configuration { get; }

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services)
      {
         services.AddOData();
         services.AddCors();
         services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

         var connection = @"Server=(localdb)\mssqllocaldb;Database=Bible;Trusted_Connection=True;ConnectRetryCount=0";
         services.AddDbContext<BibleContext>
             (options => options.UseSqlServer(connection));

         // configure strongly typed settings objects
         var appSettingsSection = Configuration.GetSection("AppSettings");
         services.Configure<AppSettings>(appSettingsSection);

         // configure jwt authentication
         var appSettings = appSettingsSection.Get<AppSettings>();
         var key = Encoding.ASCII.GetBytes(appSettings.Secret);
         services.AddAuthentication(x =>
         {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
         })
         .AddJwtBearer(x =>
         {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
               ValidateIssuerSigningKey = true,
               IssuerSigningKey = new SymmetricSecurityKey(key),
               ValidateIssuer = false,
               ValidateAudience = false
            };
         });

         // configure DI for application services
         services.AddScoped<IUserService, UserService>();
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }
         else
         {
            app.UseHsts();
         }

         loggerFactory.AddConsole(Configuration.GetSection("Logging"));
         loggerFactory.AddDebug();

         // global cors policy
         app.UseCors(x => x
             .AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader()
             .AllowCredentials());

         app.UseAuthentication();

         app.UseHttpsRedirection();
         app.UseMvc(b =>
         {
            b.MapODataServiceRoute("odata", "odata", GetEdmModel());
         });
      }

      private static IEdmModel GetEdmModel()
      {
         ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
         builder.EntitySet<BibleVersion>("BibleVersions");
         builder.EntitySet<Verse>("Verses");
         return builder.GetEdmModel();
      }
   }
}
