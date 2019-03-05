using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integratieproject1.Domain.Datatypes;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Projects;
using Integratieproject1.Domain.Surveys;
using Integratieproject1.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Integratieproject1.DAL
{
  public class CityOfIdeasDbInitializer
  {
    private static bool hasRunDuringAppExecution = false;
    public static void Initialize(CityOfIdeasDbContext context
    , bool dropCreateDatabase = false)
    {
      if (!hasRunDuringAppExecution)
      {
       
        if (dropCreateDatabase)
          context.Database.EnsureDeleted();
        
        if (context.Database.EnsureCreated())
          Seed(context);
        
        hasRunDuringAppExecution = true;
      }
      
    }

    private static void Seed(CityOfIdeasDbContext ctx)
    {
      var previousBehaviour = ctx.ChangeTracker.QueryTrackingBehavior;
      // Stel gedrag 'tracked-entities' in
      ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
      
      Address address = new Address {City = "testCity", Street = "testStreet", HouseNr = "1", ZipCode = "0000"};
      Location location = new Location {Address = address, LocationName = "test1"};
      Position position = new Position {Altitude = 0.0, Longitude = 0.0};
      
      
      Platform platform = new Platform
      {
        PlatformName = "test1",
        Address = address
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
      Project project2 = new Project
      {
        ProjectName = "test2",
        StartDate = DateTime.Today,
        EndDate = DateTime.Today.AddYears(1),
        Platform = platform,
        Objective = "test2",
        Description = "test2",
        Status = "Phase2",
        Location = location
      };
      Phase phase = new Phase
      {
        PhaseNr = 1,
        PhaseName = "phasetest1",
        Description = "phasedescriptiontest1",
        StartDate = DateTime.Today,
        EndDate = DateTime.Today.AddMonths(1),
        Project = project
      };
      Ideation ideation = new Ideation
      {
        CentralQuestion = "ideationtest1", 
        InputIdeation = true, 
        Phase = phase
      };
      

      Person loggedInUser = new Person()
      {
        Email = "test1@test.com",
        Platform = platform,
        Password = "test1",
        RoleType = RoleType.LOGGEDINUSER,
        ZipCode = "0000",
        Verified = false,
        LastName = "test1",
        FirstName = "test1",
        BirthDate = new DateTime(1000,1,1)
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

      ctx.Answers.Add(answer);
      question.Answers = new List<Answer>(){answer};
      ctx.Questions.Add(question);
      survey.Questions.Add(question);
      ctx.Surveys.Add(survey);
      phase.Surveys.Add(survey);
      ctx.Ideas.Add(idea);
      ideation.Ideas.Add(idea);
      ctx.Ideations.Add(ideation);
      phase.Ideations.Add(ideation);
      ctx.Phases.Add(phase);
      project.Phases.Add(phase);
      ctx.Projects.AddRange(project,project2);
      platform.Projects= new List<Project>{project, project2};
      ctx.Platforms.Add(platform);
            
      
      
      //platform.Projects.Add(project2);    
      
      
      
      
      
      
      
       
      ctx.SaveChanges();
      Console.WriteLine("seed");
      
      foreach (var entry in ctx.ChangeTracker.Entries().ToList())
        entry.State = EntityState.Detached;
            
      // Herstel gedrag 'ChangTracker.QueryTrackingBehavior'
      ctx.ChangeTracker.QueryTrackingBehavior = previousBehaviour;
    }
  }
}
