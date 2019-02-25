using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Integratieproject1.DAL;
using Integratieproject1.DAL.Repositorys;
using Integratieproject1.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite.Internal.ApacheModRewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Integratieproject1
{
    public class Program
  
    {
   
        public static void Main(string[] args)
        {
      //CityOfIdeasDbContext ctx = new CityOfIdeasDbContext();
      //CityOfIdeasDbInitializer.Initialize(ctx, true);
      Ideation ideation = new Ideation();
      ideation.CentralQuestion = "test";
      ideation.InputIdeation = false;
      IdeationRepository ideationRepository = new IdeationRepository();
      ideationRepository.CreateIdeation(ideation);
      CreateWebHostBuilder(args).Build().Run();
      Console.WriteLine("Hallo iedereen! Test Branching");
            
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
