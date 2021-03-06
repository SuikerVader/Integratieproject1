﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integratieproject1.Areas.Identity.Pages.Account;
using Integratieproject1.BL.Managers;
using Integratieproject1.Domain.Datatypes;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.IoT;
using Integratieproject1.Domain.Projects;
using Integratieproject1.Domain.Surveys;
using Integratieproject1.Domain.Users;
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
            , bool dropCreateDatabase = true)
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

            Address addressAntwerp = new Address {City = "Antwerpen", Street = "Groenplaats", HouseNr = "1", ZipCode = "2000"};
            Address addressGent = new Address { City = "Gent", Street = "Jan van Gentstraat", HouseNr = "1", ZipCode = "9000" };
            Address addressOssenmarkt = new Address { City = "Antwerpen", Street = "Ossenmarkt", HouseNr = "1", ZipCode = "2000" };
            Address addressVrijdagmarkt = new Address { City = "Antwerpen", Street = "Vrijdagmarkt", HouseNr = "4", ZipCode = "2000" };
            Address addressZwembad = new Address { City = "Antwerpen", Street = "Veldstraat", HouseNr = "83", ZipCode = "2000" };
            Location locationOssenmarkt = new Location {Address = addressOssenmarkt, LocationName = "Ossenmarkt"};
            Location locationVrijdagmarkt = new Location { Address = addressVrijdagmarkt, LocationName = "Vrijdagmarkt" };
            Location locationAntwerp = new Location { Address = addressAntwerp, LocationName = "Skatepark" };
            Location locationZwembad = new Location { Address = addressZwembad, LocationName = "Zwembad Veldstraat" };
            Position position = new Position {Lat = "0.0", Lng = "0.0"};

            #region Platforms
            Platform platformAntwerp = new Platform
            {
                PlatformName = "Antwerpen",
                Description =
                    "Bent u Antwerpenaar in hart en nieren? Beslis mee over uw stad!" +
                    " U kan hier meestemmen op projecten in uw buurt en zelf je eigen ideeën plaatsen!",
                Address = addressAntwerp,
                Phonenumber = "0488643152",
                BackgroundImage = "/images/uploads/BgImgAntwerp.jpg",
                Logo = "/images/uploads/LogoAntwerp.png",
                BackgroundColor = "white",
                ButtonColor = "#007bff",
                TextColor = "black"
            };

            Platform platformGent = new Platform
            {
                PlatformName = "Gent",
                Description =
                    "Bent u van Gent? Wilt u meebeslissen bij bepaalde projecten in uw eigen stad?" +
                    " Dan kan u hier meestemmen op projecten in uw buurt en zelf je eigen ideeën plaatsen!",
                Address = addressGent,
                Phonenumber = "0488644400",
                BackgroundImage = "/images/uploads/BgImgGent.jpg",
                Logo = "/images/uploads/LogoGent.png",
                BackgroundColor = "#dbffdf",
                ButtonColor = "#0f9b1f",
                TextColor = "black"
            };

            #endregion
            #region Projects
            Project projectOssenmarkt = new Project
            {
                ProjectName = "Ossenmarkt",
                StartDate = new DateTime(2017, 3, 1, 7, 0, 0),
                EndDate = new DateTime(2019, 6, 16, 12, 0, 0),
                Platform = platformAntwerp,
                Description = "Dit is dé plek waar de studenten 's middags een broodje komen eten" +
                " of een pintje komen pakken. Daarom zoeken we een idee voor een project om dit plein nog" +
                " aantrekkelijker te maken voor jongeren.",
                Status = "Phase3",
                Location = locationOssenmarkt,
                BackgroundImage = "/images/uploads/BgImgOssenmarkt.jpg"
            };
            Project projectVrijdagmarkt = new Project
            {
                ProjectName = "Vrijdagmarkt",
                StartDate = new DateTime(2019, 1, 15, 22, 30, 0),
                EndDate = new DateTime(2021, 5, 25, 14, 0, 0),
                Platform = platformAntwerp,
                Description = " Op vrijdagvoormiddag kan je hier begrijpelijkerwijs over de koppen lopen," +
                " maar op elk ander moment is de Vrijdagmarkt een oase van rust in de modebuurt van Antwerpen." +
                " Daarom willen wij hier een nieuw project opstarten om het nog aantrekkelijker te maken." +
                " Help jij ons mee?",
                Status = "Phase1",
                Location = locationVrijdagmarkt,
                BackgroundImage = "/images/uploads/BgImgVrijdagmarkt.jpg"
            };

            Project projectSkatepark = new Project
            {
                ProjectName = "Skatepark",
                StartDate = new DateTime(2017, 1, 15, 22, 30, 0),
                EndDate = new DateTime(2019, 3, 6, 17, 0, 0),
                Platform = platformAntwerp,
                Description = "In het centrum van Antwerpen zouden we graag een nieuw skatepark plaatsen. We zoeken jouw hulp!",
                Status = "Phase3",
                Location = locationAntwerp,
                BackgroundImage = "/images/uploads/BgImgSkatepark.png"
            };

            Project projectZwembad = new Project
            {
                ProjectName = "Zwembad Veldstraat",
                StartDate = new DateTime(2020, 1, 1, 0, 0, 0),
                EndDate = new DateTime(2021, 1, 1, 0, 0, 0),
                Platform = platformAntwerp,
                Description = "We hebben goed niews voor alle waterratten onder jullie! De stad Antwerpen heeft namelijk besloten om het zwembad te renoveren!",
                Status = "Startfase",
                Location = locationZwembad,
                BackgroundImage = "/images/uploads/BgImgZwembad.jpg"
            };

            #endregion
            #region Phases
            Phase phaseOssenmarkt1 = new Phase
            {
                PhaseNr = 1,
                PhaseName = "Startfase",
                Description = "In de allereerste fase van dit project zouden we graag algemene ideeën krijgen " +
                "van jullie om de Ossenmarkt aantrekkelijker te maken voor jongeren.",
                StartDate = projectOssenmarkt.StartDate,
                EndDate = new DateTime(2018, 8, 17, 22, 30, 0),
                Project = projectOssenmarkt
            };
            Phase phaseOssenmarkt2 = new Phase
            {
                PhaseNr = 2,
                PhaseName = "Fase Sport",
                Description = "Na de eerste fase zijn we tot de conclusie gekomen om het project op de Ossenmarkt " +
                "in verband met sport te maken.",
                StartDate = phaseOssenmarkt1.EndDate,
                EndDate = new DateTime(2019, 2, 2, 22, 30, 0),
                Project = projectOssenmarkt
            };
            Phase phaseOssenmarkt3 = new Phase
            {
                PhaseNr = 3,
                PhaseName = "Fase Fitness",
                Description = "Na de vorige fase was het populairste idee om een fitness buiten te plaatsen op de Ossenmarkt.",
                StartDate = phaseOssenmarkt2.EndDate,
                EndDate = projectOssenmarkt.EndDate,
                Project = projectOssenmarkt
            };

            Phase phaseVrijdagmarkt1 = new Phase
            {
                PhaseNr = 1,
                PhaseName = "Startfase",
                Description = "Bij deze fase zoeken we algemene ideeën om de vrijdagmarkt op te fleuren en aantrekkelijker te maken.",
                StartDate = projectVrijdagmarkt.StartDate,
                EndDate = new DateTime(2019, 6, 21, 5, 0, 0),
                Project = projectVrijdagmarkt
            };
            Phase phaseVrijdagmarkt2 = new Phase
            {
                PhaseNr = 2,
                PhaseName = "Eindfase",
                Description = "Fase nog niet begonnen.",
                StartDate = phaseVrijdagmarkt1.EndDate,
                EndDate = projectVrijdagmarkt.EndDate,
                Project = projectVrijdagmarkt
            };

            Phase phaseSkatepark1 = new Phase
            {
                PhaseNr = 1,
                PhaseName = "Startfase",
                Description = "Om onze lokale skaters meer plaats te geven om hun coole tricks te showen zijn ze" +
                " van plan om een nieuw skatepark te bouwen.",
                StartDate = projectSkatepark.StartDate,
                EndDate = new DateTime(2017, 12, 21, 5, 0, 0),
                Project = projectVrijdagmarkt
            };

            Phase phaseSkatepark2 = new Phase
            {
                PhaseNr = 2,
                PhaseName = "Fase Locatie",
                Description = "Na de eerste fase hebben we al een mooie locatie gevonden: Het Stadspark!",
                StartDate = phaseSkatepark1.EndDate,
                EndDate = new DateTime(2018, 10, 7, 5, 0, 0),
                Project = projectVrijdagmarkt
            };

            Phase phaseSkatepark3 = new Phase
            {
                PhaseNr = 3,
                PhaseName = "Fase Ontwerp",
                Description = "Nu we het ontwerp van het skatepark kennen hebben we nog enkele kleine details van jullie nodig.",
                StartDate = phaseSkatepark2.EndDate,
                EndDate = projectSkatepark.EndDate,
                Project = projectVrijdagmarkt
            };

            Phase phaseZwembad1 = new Phase
            {
                PhaseNr = 1,
                PhaseName = "Startfase",
                Description = "Deze fase is nog steeds in verwerking!",
                StartDate = projectZwembad.StartDate,
                EndDate = projectZwembad.EndDate,
                Project = projectVrijdagmarkt
            };

            #endregion
            #region Ideations
            Ideation ideationOssenmarktThema = new Ideation
            {
                CentralQuestion = "Rond welk thema zouden jullie het project willen laten draaien?",
                InputIdeation = true,
                Phase = phaseOssenmarkt1
            };
            Ideation ideationOssenmarktSport = new Ideation
            {
                CentralQuestion = "Welke sport zouden jullie graag op de Ossenmarkt beoefenen?",
                InputIdeation = false,
                Phase = phaseOssenmarkt2
            };
            Ideation ideationOssenmarktFitness = new Ideation
            {
                CentralQuestion = "Welke fitnesstoestellen zouden jullie graag willen plaatsen op het plein?",
                InputIdeation = true,
                Phase = phaseOssenmarkt3,
                ExternalLink = "https://www.antwerpen.be/nl/home"
                
            };

            #endregion

            #region Persons
            CustomUser person = new CustomUser
            {
                UserName = "Albert",
                Email = "testPerson1@test.com"
            };
            CustomUser organisation = new CustomUser
            {
                UserName = "McDonalds",
                Email = "testOrganisation1@test.com",
                Verified = true,
                AskVerify = false
            };
            CustomUser admin = new CustomUser
            {
                UserName = "TestAdmin",
                Email = "testAdmin1@test.com"
            };

            #endregion
            #region Ideas
            Idea ideaThema1 = new Idea
            {
                Title = "Sport",
                Ideation = ideationOssenmarktThema,
                IdentityUser = admin,
            };
            Idea ideaThema2 = new Idea
            {
                Title = "School",
                Ideation = ideationOssenmarktThema,
                IdentityUser = admin
            };
            Idea ideaThema3 = new Idea
            {
                Title = "Liefde",
                Ideation = ideationOssenmarktThema,
                IdentityUser = admin
            };
            Idea ideaSport1 = new Idea
            {
                Title = "Ping Pong",
                Ideation = ideationOssenmarktSport,
                IdentityUser = admin
            };
            Idea ideaSport2 = new Idea
            {
                Title = "Fitness",
                Ideation = ideationOssenmarktSport,
                IdentityUser = admin
            };
            Idea ideaSport3 = new Idea
            {
                Title = "Voetbal",
                Ideation = ideationOssenmarktSport,
                IdentityUser = admin
            };
            Idea ideaSport4 = new Idea
            {
                Title = "Tennis",
                Ideation = ideationOssenmarktSport,
                IdentityUser = admin
            };
            Idea ideaSport5 = new Idea
            {
                Title = "Armworstelen",
                Ideation = ideationOssenmarktSport,
                IdentityUser = admin
            };
            Idea ideaFitness1 = new Idea
            {
                Title = "Pull Up Station",
                Ideation = ideationOssenmarktFitness,
                IdentityUser = admin
            };
            Idea ideaFitness2 = new Idea
            {
                Title = "Squat Rack",
                Ideation = ideationOssenmarktFitness,
                IdentityUser = admin
            };
            Idea ideaFitness3 = new Idea
            {
                Title = "Losse Gewichten",
                Ideation = ideationOssenmarktFitness,
                IdentityUser = admin
            };

            #endregion
            #region TextFields
            TextField textfieldSport = new TextField
            {
                Text = "Jongeren laten sporten brengt alleen maar voordelen met zich mee.",
                Idea = ideaThema1,
                OrderNr = 1
            };
            TextField textfieldSchool = new TextField
            {
                Text = "School verderzetten na de schooluren! Daar staat elke student op te wachten.",
                Idea = ideaThema2,
                OrderNr = 1
            };

            TextField textfieldLiefde = new TextField
            {
                Text = "Liefde overwint alles en liefde als thema gaat meer mensen aantrekken naar het plein!",
                Idea = ideaThema3,
                OrderNr = 1
            };
            TextField textPullup = new TextField
            {
                Text = "Pullups is de ideale oefening om buiten in het zonnetje te doen!",
                Idea = ideaFitness1,
                OrderNr = 3
            };
            #endregion
            #region Videos
            Video videoPullup = new Video
            {
                Url = "https://www.youtube.com/embed/0aWOgrQeaHE",
                Idea = ideaFitness1,
                OrderNr = 1
            };
            #endregion
            #region Images
            Image imagePullup = new Image
            {
                ImageName = "Pull Up Station",
                ImagePath = "/images/uploads/ImgPullup.jpg",
                Idea = ideaFitness1,
                OrderNr = 2
            };
            Image imageSquat = new Image
            {
                ImageName = "Squat Rack",
                ImagePath = "/images/uploads/ImgSquat.jpg",
                Idea = ideaFitness2,
                OrderNr = 1
            };
            #endregion
            #region Survey

            Survey survey = new Survey
            {
                Title = "Vragenlijst Ossenmarkt",
                Phase = phaseOssenmarkt1
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
            
            Survey survey2 = new Survey
            {
                Title = "Vragenlijst Vrijdagmarkt",
                Phase = phaseVrijdagmarkt1
            };
            Question openQuestion2 = new Question
            {
                QuestionNr = 1,
                Survey = survey2,
                QuestionType = QuestionType.OPEN,
                QuestionText = "Wat is het belangrijkste voor dit plein?"
            };

            #endregion
            #region AdminProjects
            AdminProject adminProject = new AdminProject
            {
                Admin = admin,
                Project = projectOssenmarkt
            };
            AdminProject adminProject2 = new AdminProject
            {
                Admin = admin,
                Project = projectVrijdagmarkt
            };

            #endregion
            #region Reactions
            Reaction reactionPullup1 = new Reaction
            {
                Idea = ideaFitness1,
                IdentityUser = admin,
                ReactionText = "Mooi toestel om je rug mee te trainen!"
            };
            Reaction reactionPullup2 = new Reaction
            {
                Idea = ideaFitness1,
                IdentityUser = person,
                ReactionText = "Ja vind ik ook echt super!"
            };

            Reaction reactionPullup3 = new Reaction
            {
                Idea = ideaFitness1,
                IdentityUser = organisation,
                ReactionText = "Nee liever niet..."
            };

            Reaction reactionSquat1 = new Reaction
            {
                Idea = ideaFitness2,
                IdentityUser = person,
                ReactionText = "Never skip legday"
            };

            Reaction reactionSport = new Reaction
            {
                Ideation = ideationOssenmarktSport,
                IdentityUser = person,
                ReactionText = "Sporten is gezond! Dit is echt een goed idee."
            };
            #endregion
            #region Likes
            Like likeReactionPullup1 = new Like
            {
                IdentityUser = person,
                Reaction = reactionPullup1
            };

            Like likeReactionPullup2 = new Like
            {
                IdentityUser = organisation,
                Reaction = reactionPullup1
            };

            Like likeReactionPullup3 = new Like
            {
                IdentityUser = admin,
                Reaction = reactionPullup1
            };

            Like likeReactionPullup4 = new Like
            {
                IdentityUser = person,
                Reaction = reactionPullup2
            };
            #endregion

            #region Votes
            Vote vote1 = new Vote()
            {
                Idea = ideaFitness1,
                IdentityUser = person,
                VoteType = VoteType.VOTE
            };

            Vote vote2 = new Vote()
            {
                Idea = ideaFitness1,
                IdentityUser = admin,
                VoteType = VoteType.VOTE
            };

            Vote vote3 = new Vote()
            {
                Idea = ideaFitness1,
                IdentityUser = person,
                VoteType = VoteType.SHARE_FB
            };

            Vote vote4 = new Vote()
            {
                Idea = ideaFitness1,
                IdentityUser = person,
                VoteType = VoteType.SHARE_TW
            };

            Vote vote5 = new Vote()
            {
                Idea = ideaFitness2,
                IdentityUser = person,
                VoteType = VoteType.VOTE
            };

            Vote vote6 = new Vote()
            {
                Idea = ideaFitness2,
                IdentityUser = admin,
                VoteType = VoteType.VOTE
            };

            Vote vote7 = new Vote()
            {
                Idea = ideaFitness3,
                IdentityUser = person,
                VoteType = VoteType.VOTE
            };
            #endregion

            #region Tags

            Tag tag1 = new Tag()
            {
                TagName = "Sport"
            };
            Tag tag2 = new Tag()
            {
                TagName = "Kinderen"
            };
            Tag tag3 = new Tag()
            {
                TagName = "Cultuur"
            };
            Tag tag4 = new Tag()
            {
                TagName = "Jongeren"
            };
            IdeaTag ideaTag1 = new IdeaTag()
            {
                Tag = tag1,
                Idea = ideaThema1
            };
            IdeaTag ideaTag2 = new IdeaTag()
            {
                Tag = tag2,
                Idea = ideaThema2,
            };
            IdeaTag ideaTag3 = new IdeaTag()
            {
                Tag = tag3,
                Idea = ideaThema2,
            };
            IdeaTag ideaTag4 = new IdeaTag()
            {
                Tag = tag4,
                Idea = ideaThema2,
            };

            #endregion


            #region IoT

            Position groenplaats = new Position()
            {
                Lat = "51.2189511",
                Lng = "4.40110100000004"
            };
            Position centraalStation = new Position()
            {
                Lat = "51.2171919",
                Lng = "4.421446100000026"
            };
            Position pothoekstraat = new Position()
            {
                Lat = "51.22315260000001",
                Lng = "4.436785299999997"
            };
            Position stadswaag = new Position()
            {
                Lat = "51.22365989999999",
                Lng = "4.404823699999952"
            };
            
            IoTSetup ioT1 = new IoTSetup()
            {
               Idea = ideaSport1,
               Position = groenplaats,
                Code = "1"
            };
            IoTSetup ioT2 = new IoTSetup()
            {
                Idea = ideaSport2,
                Position = centraalStation,
                Code = "2"
            };
            IoTSetup ioT3 = new IoTSetup()
            {
                Idea = ideaSport3,
                Position = pothoekstraat,
                Code = "3"
            };
            IoTSetup ioT4 = new IoTSetup()
            {
                Question = radioQuestion,
                Position = stadswaag,
                Code = "4"
            };

            

            #endregion

            projectOssenmarkt.AdminProjects = new List<AdminProject> {adminProject, adminProject2};
            platformAntwerp.Users = new List<CustomUser> {person, organisation, admin};
            ideaThema1.IdeaObjects = new List<IdeaObject>() {textfieldSport};
            ideaThema2.IdeaObjects = new List<IdeaObject>() {textfieldSchool};
            ideaThema3.IdeaObjects = new List<IdeaObject>() {textfieldLiefde};
            ideaFitness1.IdeaObjects = new List<IdeaObject>() { videoPullup, imagePullup, textPullup };
            ideaFitness1.Votes = new List<Vote>() { vote1, vote2, vote3, vote4 };
            ideaFitness2.Votes = new List<Vote>() { vote5, vote6 };
            ideaFitness3.Votes = new List<Vote>() { vote7 };
            //ctx.Answers.Add(answer);
            openQuestion.Answers = new List<Answer>() { };
            radioQuestion.Answers = new List<Answer>() {radio1, radio2, radio3, radio4};
            checkQuestion.Answers = new List<Answer>() {check1, check2, check3, check4};
            dropQuestion.Answers = new List<Answer>() {drop1, drop2, drop3, drop4};
            emailQuestion.Answers = new List<Answer>() {email};
            //ctx.Questions.Add(question);
            survey.Questions = new List<Question>()
                {openQuestion, radioQuestion, checkQuestion, dropQuestion, emailQuestion};
            survey2.Questions = new List<Question>(){openQuestion2};
            //ctx.Surveys.Add(survey);
            phaseOssenmarkt1.Surveys = new List<Survey>() {survey};
            phaseVrijdagmarkt1.Surveys = new List<Survey>(){survey2};
            //ctx.Likes.Add(like);
            reactionPullup1.Likes = new List<Like>() { likeReactionPullup1, likeReactionPullup2, likeReactionPullup3 };
            reactionPullup2.Likes = new List<Like>() { likeReactionPullup4 };
            //ctx.Reactions.Add(reaction);
            ideationOssenmarktSport.Reactions = new List<Reaction>() { reactionSport };
            ideaFitness1.Reactions = new List<Reaction>() { reactionPullup1, reactionPullup2, reactionPullup3 };
            ideaFitness2.Reactions = new List<Reaction>() { reactionSquat1 };

            ctx.Tags.AddRange(tag1, tag2, tag3, tag4);
            ideaThema1.IdeaTags = new List<IdeaTag>(){ideaTag1};
            ideaThema2.IdeaTags = new List<IdeaTag>(){ideaTag2,ideaTag3,ideaTag4};
            
            /*ctx.IoTSetups.AddRange(ioT1,ioT2,ioT3,ioT4);
            ctx.Positions.AddRange(groenplaats,pothoekstraat,centraalStation,stadswaag);*/
            ideaSport1.IoTSetups = new List<IoTSetup>(){ioT1};
            ideaSport2.IoTSetups = new List<IoTSetup>(){ioT2};
            ideaSport3.IoTSetups = new List<IoTSetup>(){ioT3};
            radioQuestion.IoTSetups = new List<IoTSetup>(){ioT4};
            //ctx.Ideas.Add(idea);
            ideationOssenmarktThema.Ideas = new List<Idea>() {ideaThema1, ideaThema2, ideaThema3};
            ideationOssenmarktSport.Ideas = new List<Idea>() {ideaSport1, ideaSport2, ideaSport3, ideaSport4, ideaSport5};
            ideationOssenmarktFitness.Ideas = new List<Idea>() {ideaFitness1, ideaFitness2, ideaFitness3};
            //ctx.Ideations.Add(ideation);
            phaseOssenmarkt1.Ideations = new List<Ideation>() {ideationOssenmarktThema};
            phaseOssenmarkt2.Ideations = new List<Ideation>() {ideationOssenmarktSport};
            phaseOssenmarkt3.Ideations = new List<Ideation>() {ideationOssenmarktFitness};
            //ctx.Phases.Add(phase);
            projectOssenmarkt.Phases = new List<Phase>() {phaseOssenmarkt1, phaseOssenmarkt2, phaseOssenmarkt3};
            projectVrijdagmarkt.Phases = new List<Phase>() {phaseVrijdagmarkt1, phaseVrijdagmarkt2};
            projectSkatepark.Phases = new List<Phase>() { phaseSkatepark1, phaseSkatepark2, phaseSkatepark3 };
            projectZwembad.Phases = new List<Phase>() { phaseZwembad1 };
                //ctx.Projects.AddRange(project,project2);
            platformAntwerp.Projects = new List<Project>() {projectOssenmarkt, projectVrijdagmarkt, projectSkatepark, projectZwembad};
            ctx.Platforms.AddRange(platformAntwerp, platformGent);

            //platform.Projects.Add(project2);    

            ctx.SaveChanges();
            Console.WriteLine("seed");

            foreach (var entry in ctx.ChangeTracker.Entries().ToList())
                entry.State = EntityState.Detached;

            // Herstel gedrag 'ChangTracker.QueryTrackingBehavior'
            ctx.ChangeTracker.QueryTrackingBehavior = previousBehaviour;
        }

        public static async Task SeedUsers(UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager)
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
            var superAdminTest = new CustomUser {UserName = "Superadmin", Email = "superadmin@gmail.com", EmailConfirmed=true, Name = "Super", Surname = "Admin", Sex = "Male", Age = 35, Zipcode = "2275"};
            var adminTest = new CustomUser {UserName = "Admin", Email = "admin@gmail.com", EmailConfirmed = true };
            var modTest = new CustomUser {UserName = "Mod", Email = "mod@gmail.com", EmailConfirmed = true };
            var organisationTest = new CustomUser
                {UserName = "Organisation", Email = "organisation@gmail.com", EmailConfirmed = true };
            var userTest = new CustomUser {UserName = "User", Email = "user@gmail.com", EmailConfirmed = true, Age = 21};

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