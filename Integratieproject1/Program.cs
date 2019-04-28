using System;
using Integratieproject1.BL.Managers;
using Integratieproject1.DAL;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Integratieproject1
{
    public class Program

    {
        public static void Main(string[] args)
        {
        /*CityOfIdeasDbContext ctx = new CityOfIdeasDbContext();
        ProjectsManager projectsManager = new ProjectsManager();
        Console.WriteLine(projectsManager.GetPlatform(1).Projects.Count);*/
        CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
