using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Integratieproject1.BL.Managers;
using Integratieproject1.BL.Models.Datatypes;
using Integratieproject1.BL.Models.Ideations;
using Integratieproject1.BL.Models.Projects;
using Integratieproject1.BL.Models.Surveys;
using Integratieproject1.BL.Models.Users;
using Integratieproject1.DAL;
using Integratieproject1.DAL.Repositories;
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
       //CityOfIdeasDbContext dbContext = new CityOfIdeasDbContext();
       /*ProjectsManager projectsManager = new ProjectsManager();
       Platform platform;
       platform = projectsManager.GetPlatform(1);
       Console.WriteLine(platform.PlatformName);
       Console.WriteLine(platform.Projects.Count);*/
      CreateWebHostBuilder(args).Build().Run();
      Console.WriteLine("Hallo iedereen! Test Branching");
            
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
