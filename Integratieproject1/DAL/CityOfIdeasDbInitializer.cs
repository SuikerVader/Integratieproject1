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
    , bool dropCreateDatabase = true)
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
        Title = "SurveyTest",
        Phase = phase
      };
      Question openQuestion = new Question
      {
        QuestionNr = 1,
        Survey = survey,
        QuestionText = "Wat is het belangrijkste voor dit plein?"
      };
      
      Question radioQuestion = new Question
      {
        QuestionNr = 2,
        Survey = survey,
        QuestionText = "Voor wie is het plein het belangrijkste?"
      };
      
      Question checkQuestion = new Question
      {
        QuestionNr = 3,
        Survey = survey,
        QuestionText = "Wat zou je graag willen doen op dit plein?"
      };
      
      Question dropQuestion = new Question
      {
        QuestionNr = 4,
        Survey = survey,
        QuestionText = "Hoe belangrijk is dit plein voor jou?"
      };
      
      Question emailQuestion = new Question
      {
        QuestionNr = 5,
        Survey = survey,
        QuestionText = "Geef je email om je stem te bevestigen!"
      };
      
      Answer open = new Answer
      {
        TotalTimesChosen = 0,
        Question = openQuestion,
        AnswerType = AnswerType.OPEN,
        AnswerText = ""
      };
      
      Answer radio1 = new Answer
      {
        TotalTimesChosen = 0,
        Question = radioQuestion,
        AnswerType = AnswerType.RADIO,
        AnswerText = "Jongeren"
      };
      
      Answer radio2 = new Answer
      {
        TotalTimesChosen = 0,
        Question = radioQuestion,
        AnswerType = AnswerType.RADIO,
        AnswerText = "Volwassenen"
      };
      
      Answer radio3 = new Answer
      {
        TotalTimesChosen = 0,
        Question = radioQuestion,
        AnswerType = AnswerType.RADIO,
        AnswerText = "Ouderen"
      };
      
      Answer radio4 = new Answer
      {
        TotalTimesChosen = 0,
        Question = radioQuestion,
        AnswerType = AnswerType.RADIO,
        AnswerText = "Iedereen"
      };
      
      Answer check1 = new Answer
      {
        TotalTimesChosen = 0,
        Question = checkQuestion,
        AnswerType = AnswerType.CHECK,
        AnswerText = "Sporten"
      };
      
      Answer check2 = new Answer
      {
        TotalTimesChosen = 0,
        Question = checkQuestion,
        AnswerType = AnswerType.CHECK,
        AnswerText = "Spelen"
      };
      
      Answer check3 = new Answer
      {
        TotalTimesChosen = 0,
        Question = checkQuestion,
        AnswerType = AnswerType.CHECK,
        AnswerText = "Ontspannen"
      };
      
      Answer check4 = new Answer
      {
        TotalTimesChosen = 0,
        Question = checkQuestion,
        AnswerType = AnswerType.CHECK,
        AnswerText = "Geen mening"
      };
      
      Answer drop1 = new Answer
      {
        TotalTimesChosen = 0,
        Question = dropQuestion,
        AnswerType = AnswerType.DROP,
        AnswerText = "Niet belangrijk"
      };
      
      Answer drop2 = new Answer
      {
        TotalTimesChosen = 0,
        Question = dropQuestion,
        AnswerType = AnswerType.DROP,
        AnswerText = "Beetje belangrijk"
      };
      
      Answer drop3 = new Answer
      {
        TotalTimesChosen = 0,
        Question = dropQuestion,
        AnswerType = AnswerType.DROP,
        AnswerText = "Vrij belangrijk"
      };
      
      Answer drop4 = new Answer
      {
        TotalTimesChosen = 0,
        Question = dropQuestion,
        AnswerType = AnswerType.DROP,
        AnswerText = "Heel belangrijk"
      };
      
      Answer email = new Answer
      {
        TotalTimesChosen = 0,
        Question = emailQuestion,
        AnswerType = AnswerType.EMAIL,
        AnswerText = ""
      };
      Reaction reaction = new Reaction
      {
        Idea = idea,
        TotalLikes = 1,
        ReactionText = "reactionTest1",
        LoggedInUser = loggedInUser
      };
      Vote vote = new Vote
      {
        VoteType = VoteType.VOTE,
        User = loggedInUser,
        Confirmed = true,
        Idea = idea
      };

      idea.Reactions = new List<Reaction>(){reaction};
      idea.Votes = new List<Vote>(){vote};
      //ctx.Answers.Add(answer);
      openQuestion.Answers = new List<Answer>(){open};
      radioQuestion.Answers = new List<Answer>(){radio1, radio2, radio3, radio4};
      checkQuestion.Answers = new List<Answer>(){check1, check2, check3, check4};
      dropQuestion.Answers = new List<Answer>(){drop1, drop2, drop3, drop4};
      emailQuestion.Answers = new List<Answer>(){email};
      //ctx.Questions.Add(question);
      survey.Questions = new List<Question>(){openQuestion, radioQuestion, checkQuestion, dropQuestion, emailQuestion};
      //ctx.Surveys.Add(survey);
      phase.Surveys = new List<Survey>(){survey};
      //ctx.Ideas.Add(idea);
      ideation.Ideas = new List<Idea>(){idea};
      //ctx.Ideations.Add(ideation);
      phase.Ideations = new List<Ideation>(){ideation};
      //ctx.Phases.Add(phase);
      project.Phases = new List<Phase>() {phase};
      //ctx.Projects.AddRange(project,project2);
      platform.Projects = new List<Project>(){project, project2};
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
