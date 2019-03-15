using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Integratieproject1.BL.Managers;
using Integratieproject1.Domain.Datatypes;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Projects;
using Integratieproject1.Domain.Surveys;
using Integratieproject1.Domain.Users;
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
<<<<<<< HEAD
            CityOfIdeasDbContext ctx = new CityOfIdeasDbContext();
            CityOfIdeasDbInitializer.Initialize(ctx, true);

            Address address = new Address {City = "testCity", Street = "testStreet", HouseNr = "1", ZipCode = "0000"};
            Location location = new Location {Address = address, LocationName = "test1"};
            Position position = new Position {Altitude = 0.0, Longitude = 0.0};

            Platform platform = new Platform
            {
                PlatformName = "test1",
                Adress = address
            };

            Project project = new Project
            {
                ProjectName = "test1",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddYears(1),
                Platform = platform,
                Objective = "test1",
                Description = "test1",
                Status = "Phase1",
                Location = location
            };
            Phase phase = new Phase
            {
                PhaseNr = 1,
                PhaseName = "test1",
                Description = "test1",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(1),
                Project = project
            };

            Ideation ideation = new Ideation
            {
                CentralQuestion = "test1",
                InputIdeation = false,
                Phase = phase
            };

            IdeationsRepository ideationsRepository = new IdeationsRepository();
            ProjectsRepository projectsRepository = new ProjectsRepository();
            projectsRepository.CreatePlatform(platform);
            projectsRepository.CreateProject(project);
            projectsRepository.CreatePhase(phase);
            //ideationsRepository.CreateIdeation(ideation);


            CreateWebHostBuilder(args).Build().Run();
            Console.WriteLine("Hallo iedereen! Test Branching");
=======
        /*CityOfIdeasDbContext ctx = new CityOfIdeasDbContext();
        ProjectsManager projectsManager = new ProjectsManager();
        Console.WriteLine(projectsManager.GetPlatform(1).Projects.Count);*/
        CreateWebHostBuilder(args).Build().Run();
>>>>>>> a81d43807e7aa11edcdc5bbcc7f6c0d679788026
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}