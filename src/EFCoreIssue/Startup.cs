﻿using EFCoreIssue.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EFCoreIssue
{
  public class Startup
  {
    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
          .AddEnvironmentVariables();
      Configuration = builder.Build();
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<ApplicationDbContext>(options =>
      {
        options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]);
      }, ServiceLifetime.Scoped);

      services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
      {
        options.Cookies.ApplicationCookie.AutomaticChallenge = false;
        options.Cookies.ApplicationCookie.CookieHttpOnly = true;
        options.Cookies.ApplicationCookie.CookieSecure = CookieSecurePolicy.SameAsRequest; // Always wäre ja schöner, ist aber für Tests und Debugging schwierig
        options.Password.RequireNonAlphanumeric = false;
      })
        .AddEntityFrameworkStores<ApplicationDbContext, long>()
        .AddDefaultTokenProviders();

      services.AddMvc();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
      }

      app.UseStaticFiles();

      app.UseIdentity();

      app.UseMvc(routes =>
      {
        routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}
