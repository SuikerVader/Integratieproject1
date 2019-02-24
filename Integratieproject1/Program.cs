using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Integratieproject1.DAL;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Integratieproject1
{
    public class Program
  
    {
   
        public static void Main(string[] args)
        {
      CityOfIdeasDbContext ctx = new CityOfIdeasDbContext();
      CityOfIdeasDbInitializer.Initialize(ctx, true);
      CreateWebHostBuilder(args).Build().Run();
      Console.WriteLine("Hallo iedereen! Test Branching");
            
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
