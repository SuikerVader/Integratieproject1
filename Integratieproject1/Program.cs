using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Integratieproject1.BL.Models.Datatypes;
using Integratieproject1.BL.Models.Ideations;
using Integratieproject1.BL.Models.Projects;
using Integratieproject1.BL.Models.Surveys;
using Integratieproject1.BL.Models.Users;
using Integratieproject1.DAL;
using Integratieproject1.DAL.Repositorys;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite.Internal.ApacheModRewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Integratieproject1
{
    public class Program

    {
        public static UnitOfWork uow = new UnitOfWork();
        public static void Main(string[] args)
        {
       
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
          InputIdeation = true, 
          Phase = phase
      };

      LoggedInUser loggedInUser = new LoggedInUser
      {
          Email = "test1@test.com",
          Platform = platform,
          Password = "test1",
          RoleType = RoleType.LOGGEDINUSER,
          ZipCode = "0000",
          Verified = false,
      };
      
      Idea idea = new Idea
      {
          Title = "test1",
          Ideation = ideation,
          LoggedInUser = loggedInUser
      };
      Survey survey = new Survey
      {
          Title = "test1",
          Phase = phase
      };
      Question question = new Question
      {
          QuestionNr = 1,
          Survey = survey,
          QuestionText = "test1"
      };
      Answer answer = new Answer
      {
          TotalTimesChosen = 0,
          Question = question,
          AnswerText = "test1"
      };
      
      
      
      ProjectsRepository projectsRepository  = new ProjectsRepository(uow.ctx);
      projectsRepository.CreatePlatform(platform);
      projectsRepository.CreateProject(project);
      projectsRepository.CreatePhase(phase);
      IdeationsRepository ideationsRepository = new IdeationsRepository(uow.ctx);
      ideationsRepository.CreateIdeation(ideation);
      ideationsRepository.CreateIdea(idea);
      SurveysRepository surveysRepository = new SurveysRepository(uow.ctx);
      surveysRepository.CreateSurvey(survey);
      surveysRepository.CreateQuestion(question);
      surveysRepository.CreateAnswer(answer);
      
      
      CreateWebHostBuilder(args).Build().Run();
      Console.WriteLine("Hallo iedereen! Test Branching");
            
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
