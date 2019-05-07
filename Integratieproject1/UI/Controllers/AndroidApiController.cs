using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Integratieproject1.BL.Managers;
using Integratieproject1.Domain.Projects;
using Microsoft.AspNetCore.Mvc;
using Integratieproject1.DAL;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Surveys;
using Integratieproject1.Domain.Users;

namespace Integratieproject1.UI.Controllers
{
    [ApiController]
    public class AndroidApiController
    {
        private CityOfIdeasDbContext entities;

        public AndroidApiController()
        {
            entities = new CityOfIdeasDbContext();
        }

        #region Ideations

        [HttpGet]
        [Route("Api/ideas")]
        public IEnumerable<Idea> GetIdeas()
        {
            return entities.Ideas.ToArray();
        }

        [HttpGet]
        [Route("Api/ideations")]
        public IEnumerable<Ideation> GetIdeations()
        {
            return entities.Ideations.ToList();
        }

        [HttpGet]
        [Route("Api/reactions")]
        public IEnumerable<Reaction> GetReactions()
        {
            return entities.Reactions.ToList();
        }

        [HttpGet]
        [Route("Api/votes")]
        public IEnumerable<Vote> GetVotes()
        {
            return entities.Votes.ToList();
        }

        [HttpGet]
        [Route("Api/like")]
        public IEnumerable<Like> GetLikes()
        {
            return entities.Likes.ToList();
        }

        #endregion

        #region Projects

        [HttpGet]
        [Route("Api/projects")]
        public IEnumerable<Project> GetProjects()
        {
            return entities.Projects.ToList();
        }

        [HttpGet]
        [Route("Api/phase")]
        public IEnumerable<Phase> GetPhases()
        {
            return entities.Phases.ToList();
        }

        [HttpGet]
        [Route("Api/platform")]
        public IEnumerable<Platform> GetPlatform()
        {
            return entities.Platforms.ToList();
        }

        #endregion

        #region surveys

        [HttpGet]
        [Route("Api/surveys")]
        public IEnumerable<Survey> GetSurveys()
        {
            return entities.Surveys.ToList();
        }

        [HttpGet]
        [Route("Api/Answer")]
        public IEnumerable<Answer> GetAnswers()
        {
            return entities.Answers.ToList();
        }

        [HttpGet]
        [Route("Api/question")]
        public IEnumerable<Question> GetQuestions()
        {
            return entities.Questions.ToList();
        }

        #endregion

        #region users

        #endregion
        
        
        
    }
}