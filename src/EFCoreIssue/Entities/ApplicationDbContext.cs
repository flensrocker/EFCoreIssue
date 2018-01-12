using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EFCoreIssue.Entities
{
  public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
  {
    public DbSet<SomeEntity> DbsSomeEntities { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options)
    {
      PrintStackTrace();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.Entity<SomeEntity>().HasIndex(x => x.Name).IsUnique();
    }

    public override void Dispose()
    {
      PrintStackTrace();
      base.Dispose();
    }

    private void PrintStackTrace()
    {
      var st = new StackTrace();
      foreach (var sf in st.GetFrames())
        Console.WriteLine($"{sf.GetMethod().DeclaringType.Assembly.GetName().Name} {sf.GetMethod().DeclaringType.Name} {sf.GetMethod().Name}");
    }
  }
}
