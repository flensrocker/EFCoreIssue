using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace EFCoreIssue
{
  public class Program
  {
    public static void Main(string[] args)
    {
      BuildWebHost(args).Run();
    }

    public static IWebHost BuildWebHost(string[] args)
    {
      var config = new ConfigurationBuilder()
          .AddEnvironmentVariables(prefix: "ASPNETCORE_")
          .Build();

      return new WebHostBuilder()
       .UseConfiguration(config)
       .UseKestrel()
       .UseContentRoot(Directory.GetCurrentDirectory())
       .UseIISIntegration()
       .UseStartup<Startup>()
       .Build();
    }
  }
}
