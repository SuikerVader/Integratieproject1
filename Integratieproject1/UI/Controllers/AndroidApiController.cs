using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Integratieproject1.BL.Managers;
using Integratieproject1.Domain.Projects;
using Microsoft.AspNetCore.Mvc;
using Integratieproject1.DAL;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Surveys;
using Integratieproject1.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Integratieproject1.UI.Controllers
{
    [ApiController]
    public class AndroidApiController
    {
        private IdeationsManager _ideationsManager;
        private SurveysManager _surveysManager;
        private ProjectsManager _projectsManager;
        private UsersManager _usersManager;

        public AndroidApiController()
        {
            _ideationsManager = new IdeationsManager();
            _surveysManager = new SurveysManager();
            _projectsManager = new ProjectsManager();
            _usersManager = new UsersManager();
        }


        #region Ideations

        [HttpGet]
        [Route("Api/ideas/{id}")]
        public IEnumerable<Idea> GetIdeas(int id)
        {
            return _ideationsManager.GetAllIdeas(id);
        }

        [HttpGet]
        [Route("Api/ideations/{id}")]
        public IEnumerable<Ideation> GetIdeations(int id)
        {
            return _ideationsManager.GetProjectIdeation(id);
        }

        [HttpGet]
        [Route("Api/reactions/{id}")]
        public IEnumerable<Reaction> GetReactions(int id)
        {
            return _ideationsManager.GetAllReactions(id);
        }

        [HttpGet]
        [Route("Api/votes/{id}")]
        public IEnumerable<Vote> GetVotes(int id)
        {
            return null;
        }

        [HttpGet]
        [Route("Api/like/{id}")]
        public IEnumerable<Like> GetLikes(int id)
        {
            return null;
        }

        #endregion

        #region Projects
        
        [HttpGet]
        [Route("Api/projects")]
        public IEnumerable<Project> GetProjects()
        {
            return _projectsManager.GetAllProjects();
        }

        [HttpGet]
        [Route("Api/phases/{id}")]
        public IEnumerable<Phase> GetPhases(int id)
        {
            return _projectsManager.GetPhases(id);
        }

        [HttpGet]
        [Route("Api/platform/{id}")]
        public Platform GetPlatform(int id)
        {
            return _projectsManager.GetPlatform(id);
        }

        #endregion

        #region surveys

        [HttpGet]
        [Route("Api/surveys/{id}")]
        public IEnumerable<Survey> GetSurveys(int id)
        {
            return _surveysManager.GetSurveys(id);
        }

        [HttpGet]
        [Route("Api/Answer/{id}")]
        public IEnumerable<Answer> GetAnswers()
        {
            return null;
        }

        [HttpGet]
        [Route("Api/question/{id}")]
        public Question GetQuestions(int id)
        {
            return _surveysManager.GetQuestion(id);
        }

        #endregion

        #region users

        #endregion
        
        
        
    }
}