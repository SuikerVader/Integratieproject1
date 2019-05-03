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
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Platform = Integratieproject1.Domain.Projects.Platform;
using Project = Integratieproject1.Domain.Projects.Project;

namespace Integratieproject1.UI.Controllers
{
    [ApiController]
    public class AndroidApiController
    {
        private readonly IdeationsManager _ideationsManager;
        private readonly SurveysManager _surveysManager;
        private readonly ProjectsManager _projectsManager;
        private readonly UsersManager _usersManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AndroidApiController(SignInManager<IdentityUser> signInManager)
        {
            _ideationsManager = new IdeationsManager();
            _surveysManager = new SurveysManager();
            _projectsManager = new ProjectsManager();
            _usersManager = new UsersManager();
            _signInManager = signInManager;
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
        [Route("Api/projects/{id}")]
        public IEnumerable<Project> GetProjects(int id)
        {
            return _projectsManager.GetProjects(id);
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

        #region Users

        [HttpGet]
        [Route("Api/users/{email}/{password}")]
        public async Task<IdentityUser> GetUser(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, true, true);

            return result.Succeeded ? _usersManager.GetUserByEmail(email) : null;
        }

        #endregion
    }
}