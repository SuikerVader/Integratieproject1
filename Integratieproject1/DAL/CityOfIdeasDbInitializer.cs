using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integratieproject1.Areas.Identity.Pages.Account;
using Integratieproject1.Domain.Datatypes;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Projects;
using Integratieproject1.Domain.Surveys;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Integratieproject1.DAL
{
    public class CityOfIdeasDbInitializer
    {
        private static bool _hasRunDuringAppExecution = false;

        public static void Initialize(CityOfIdeasDbContext context
            , bool dropCreateDatabase = false)
        {
            if (!_hasRunDuringAppExecution)
            {
                if (dropCreateDatabase)
                    context.Database.EnsureDeleted();

                if (context.Database.EnsureCreated())
                    SeedAsync(context);

                _hasRunDuringAppExecution = true;
            }
        }

        private static async Task SeedAsync(CityOfIdeasDbContext ctx)
        {
            var previousBehaviour = ctx.ChangeTracker.QueryTrackingBehavior;
            // Stel gedrag 'tracked-entities' in
            ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

            Address address = new Address {City = "testCity", Street = "testStreet", HouseNr = "1", ZipCode = "0000"};
            Location location = new Location {Address = address, LocationName = "test1"};
            Position position = new Position {Altitude = 0.0, Longitude = 0.0};


            Platform platform = new Platform
            {
                PlatformName = "CityOfIdeas",
                Description =
                    "Help building projects inside your city! Give your ideas and input on projects that inspire you.",
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
                StartDate = DateTime.Today.AddMonths(1),
                EndDate = DateTime.Today.AddMonths(1).AddYears(1),
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
                StartDate = project.StartDate,
                EndDate = project.StartDate.AddMonths(4),
                Project = project
            };
            Phase phase2 = new Phase
            {
                PhaseNr = 2,
                PhaseName = "phasetest2",
                Description = "phasedescriptiontest2",
                StartDate = phase.EndDate,
                EndDate = phase.EndDate.AddMonths(4),
                Project = project
            };
            Phase phase3 = new Phase
            {
                PhaseNr = 3,
                PhaseName = "phasetest3",
                Description = "phasedescriptiontest3",
                StartDate = phase2.EndDate,
                EndDate = project.EndDate,
                Project = project
            };
            Ideation ideation = new Ideation
            {
                CentralQuestion = "ideationtest1",
                InputIdeation = true,
                Phase = phase
            };


            IdentityUser person = new IdentityUser
            {
                Email = "testPerson1@test.com"
            };
            IdentityUser organisation = new IdentityUser
            {
                Email = "testOrganisation1@test.com"
            };
            IdentityUser admin = new IdentityUser
            {
                Email = "testAdmin1@test.com"
            };

            Idea idea = new Idea
            {
                Title = "testIdea1",
                Ideation = ideation,
                IdentityUser = person
            };
            Idea idea2 = new Idea
            {
                Title = "testIdea2",
                Ideation = ideation,
                IdentityUser = organisation
            };
            Reaction reaction = new Reaction
            {
                Idea = idea,
                ReactionText = "reactionTest1",
                IdentityUser = person
            };
            Vote vote = new Vote
            {
                VoteType = VoteType.VOTE,
                IdentityUser = person,
                Confirmed = true,
                Idea = idea
            };
            Like like = new Like
            {
                Reaction = reaction,
                IdentityUser = person
            };

            #region Survey

            Survey survey = new Survey
            {
                Title = "SurveyTest",
                Phase = phase
            };
            Question openQuestion = new Question
            {
                QuestionNr = 1,
                Survey = survey,
                QuestionType = QuestionType.OPEN,
                QuestionText = "Wat is het belangrijkste voor dit plein?"
            };

            Question radioQuestion = new Question
            {
                QuestionNr = 2,
                Survey = survey,
                QuestionType = QuestionType.RADIO,
                QuestionText = "Voor wie is het plein het belangrijkste?"
            };

            Question checkQuestion = new Question
            {
                QuestionNr = 3,
                Survey = survey,
                QuestionType = QuestionType.CHECK,
                QuestionText = "Wat zou je graag willen doen op dit plein?"
            };

            Question dropQuestion = new Question
            {
                QuestionNr = 4,
                Survey = survey,
                QuestionType = QuestionType.DROP,
                QuestionText = "Hoe belangrijk is dit plein voor jou?"
            };

            Question emailQuestion = new Question
            {
                QuestionNr = 5,
                Survey = survey,
                QuestionType = QuestionType.EMAIL,
                QuestionText = "Geef je email om je stem te bevestigen!"
            };

            Answer open = new Answer
            {
                TotalTimesChosen = 0,
                Question = openQuestion,
                AnswerText = ""
            };

            Answer radio1 = new Answer
            {
                TotalTimesChosen = 0,
                Question = radioQuestion,
                AnswerText = "Jongeren"
            };

            Answer radio2 = new Answer
            {
                TotalTimesChosen = 0,
                Question = radioQuestion,
                AnswerText = "Volwassenen"
            };

            Answer radio3 = new Answer
            {
                TotalTimesChosen = 0,
                Question = radioQuestion,
                AnswerText = "Ouderen"
            };

            Answer radio4 = new Answer
            {
                TotalTimesChosen = 0,
                Question = radioQuestion,
                AnswerText = "Iedereen"
            };

            Answer check1 = new Answer
            {
                TotalTimesChosen = 0,
                Question = checkQuestion,
                AnswerText = "Sporten"
            };

            Answer check2 = new Answer
            {
                TotalTimesChosen = 0,
                Question = checkQuestion,
                AnswerText = "Spelen"
            };

            Answer check3 = new Answer
            {
                TotalTimesChosen = 0,
                Question = checkQuestion,
                AnswerText = "Ontspannen"
            };

            Answer check4 = new Answer
            {
                TotalTimesChosen = 0,
                Question = checkQuestion,
                AnswerText = "Geen mening"
            };

            Answer drop1 = new Answer
            {
                TotalTimesChosen = 0,
                Question = dropQuestion,
                AnswerText = "Niet belangrijk"
            };

            Answer drop2 = new Answer
            {
                TotalTimesChosen = 0,
                Question = dropQuestion,
                AnswerText = "Beetje belangrijk"
            };

            Answer drop3 = new Answer
            {
                TotalTimesChosen = 0,
                Question = dropQuestion,
                AnswerText = "Vrij belangrijk"
            };

            Answer drop4 = new Answer
            {
                TotalTimesChosen = 0,
                Question = dropQuestion,
                AnswerText = "Heel belangrijk"
            };

            Answer email = new Answer
            {
                TotalTimesChosen = 0,
                Question = emailQuestion,
                AnswerText = ""
            };

            #endregion

            AdminProject adminProject = new AdminProject
            {
                Admin = admin,
                Project = project
            };
            AdminProject adminProject2 = new AdminProject
            {
                Admin = admin,
                Project = project2
            };
            project.AdminProjects = new List<AdminProject> {adminProject, adminProject2};
            platform.Users = new List<IdentityUser> {person, organisation, admin};
            reaction.Likes = new List<Like>() {like};
            idea.Reactions = new List<Reaction>() {reaction};
            idea.Votes = new List<Vote>() {vote};
            idea.Images = new List<Image>(){};
            idea2.Images = new List<Image>(){};
            //ctx.Answers.Add(answer);
            openQuestion.Answers = new List<Answer>() {};
            radioQuestion.Answers = new List<Answer>() {radio1, radio2, radio3, radio4};
            checkQuestion.Answers = new List<Answer>() {check1, check2, check3, check4};
            dropQuestion.Answers = new List<Answer>() {drop1, drop2, drop3, drop4};
            emailQuestion.Answers = new List<Answer>() {email};
            //ctx.Questions.Add(question);
            survey.Questions = new List<Question>()
                {openQuestion, radioQuestion, checkQuestion, dropQuestion, emailQuestion};
            //ctx.Surveys.Add(survey);
            phase.Surveys = new List<Survey>() {survey};
            //ctx.Ideas.Add(idea);
            ideation.Ideas = new List<Idea>() {idea, idea2};
            //ctx.Ideations.Add(ideation);
            phase.Ideations = new List<Ideation>() {ideation};
            //ctx.Phases.Add(phase);
            project.Phases = new List<Phase>() {phase, phase2, phase3};
            //ctx.Projects.AddRange(project,project2);
            platform.Projects = new List<Project>() {project, project2};
            ctx.Platforms.Add(platform);

            //platform.Projects.Add(project2);    

            ctx.SaveChanges();
            Console.WriteLine("seed");

            foreach (var entry in ctx.ChangeTracker.Entries().ToList())
                entry.State = EntityState.Detached;

            // Herstel gedrag 'ChangTracker.QueryTrackingBehavior'
            ctx.ChangeTracker.QueryTrackingBehavior = previousBehaviour;
        }

        public static async Task SeedUsers(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Rollen aanmaken
            var superAdminRole = new IdentityRole {NormalizedName = "SuperAdmin", Name = "SuperAdmin"};
            var adminRole = new IdentityRole {NormalizedName = "Admin", Name = "Admin"};
            var modRole = new IdentityRole {NormalizedName = "Mod", Name = "Mod"};
            var userRole = new IdentityRole {NormalizedName = "User", Name = "User"};
            var organisationRole = new IdentityRole {NormalizedName = "Organisation", Name = "Organisation"};

            // Rollen opslaan
            await roleManager.CreateAsync(superAdminRole);
            await roleManager.CreateAsync(adminRole);
            await roleManager.CreateAsync(modRole);
            await roleManager.CreateAsync(organisationRole);
            await roleManager.CreateAsync(userRole);

            // TestUsers aanmaken
            var superAdminTest = new IdentityUser {UserName = "superadmin@gmail.com", Email = "superadmin@gmail.com"};
            var adminTest = new IdentityUser {UserName = "admin@gmail.com", Email = "admin@gmail.com"};
            var modTest = new IdentityUser {UserName = "mod@gmail.com", Email = "mod@gmail.com"};
            var organisationTest = new IdentityUser
                {UserName = "organisation@gmail.com", Email = "organisation@gmail.com"};
            var userTest = new IdentityUser {UserName = "user@gmail.com", Email = "user@gmail.com"};

            //Users opslaan
            await userManager.CreateAsync(superAdminTest, "SuperAdmin123!");
            await userManager.CreateAsync(adminTest, "Admin123!");
            await userManager.CreateAsync(modTest, "Mod123!");
            await userManager.CreateAsync(organisationTest, "Organisation123!");
            await userManager.CreateAsync(userTest, "User123!");

            await userManager.AddToRoleAsync(superAdminTest, "SuperAdmin");
            await userManager.AddToRoleAsync(adminTest, "Admin");
            await userManager.AddToRoleAsync(modTest, "Mod");
            await userManager.AddToRoleAsync(organisationTest, "Organisation");
            await userManager.AddToRoleAsync(userTest, "User");
        }
    }
}
