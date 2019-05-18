using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Integratieproject1.BL.Managers;
using Integratieproject1.Domain.Projects;
using Microsoft.AspNetCore.Mvc;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Surveys;
using Integratieproject1.Domain.Users;
using Microsoft.AspNetCore.Http.Headers;
using JWT;
using JWT.Serializers;
using JWT.Algorithms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
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
        private readonly SignInManager<CustomUser> _signInManager;

        public AndroidApiController(SignInManager<CustomUser> signInManager)
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
        [Route("Api/idea/{id}")]
        public Idea GetIdea(int id)
        {
            return _ideationsManager.GetIdea(id);
        }

        [HttpGet]
        [Route("Api/ideations/{id}")]
        public IEnumerable<Ideation> GetIdeations(int id)
        {
            return _ideationsManager.GetProjectIdeations(id);
        }

        [HttpGet]
        [Route("Api/ideation/{id}")]
        public Ideation GetIdeation(int id)
        {
            return _ideationsManager.GetIdeation(id);
        }

        [HttpGet]
        [Route("Api/reactions/{id}")]
        public IEnumerable<Reaction> GetIdeaReactions(int id)
        {
            return _ideationsManager.GetIdeaReactions(id);
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

//        [Authorize]
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


        [HttpPost]
        [Route("/Api/vote")]
        public void AndroidVote([FromBody] int id)
        {
            ClaimsPrincipal currentUser = ClaimsPrincipal.Current;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            _ideationsManager.CreateVote(ideaId: id, voteType: VoteType.VOTE, userId: currentUserId);
        }

        #endregion

        #region Surveys

        [HttpGet]
        [Route("Api/surveys/{id}")]
        public IEnumerable<Survey> GetProjectsSurveys(int id)
        {
            return _surveysManager.GetProjectsSurveys(id);
        }

        [HttpGet]
        [Route("Api/Answer/{id}")]
        public IEnumerable<Answer> GetAnswers()
        {
            return null;
        }

        [HttpGet]
        [Route("Api/questions/{id}")]
        public IEnumerable<Question> GetQuestions(int id)
        {
            return _surveysManager.GetQuestions(id).ToList();
        }

        #endregion

        #region Tags
        
        [HttpGet]
        [Route("Api/tags")]
        public IEnumerable<Tag> GetTags()
        {
            return _ideationsManager.GetAllTags().ToList();
        }

        #endregion
        
        #region Users

        [HttpGet]
        [Route("Api/users")]
        public IEnumerable<IdentityUser> GetUsers()
        {
            return _usersManager.GetUsers("USER");
        }

//        [HttpGet]
//        [Route("Api/users/new/{email}/{password}")]
//        public async Task<IdentityUser> CreateUser(string email, string password)
//        {
//            var newUser = new IdentityUser
//            {
//                UserName = email, 
//                Email = email
//            };
//            var result = await _userManager.CreateAsync(newUser, password);
//            _usersManager.GiveRole(newUser.Id, "USER");
//            if (result.Succeeded)
//            {
//                var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
//                var callbackUrl = .Page(
//                    "/Account/ConfirmEmail",
//                    pageHandler: null,
//                    values: new { userId = newUser.Id, code = code },
//                    protocol: Request.Scheme);
//
//                
//                await _emailSender.SendEmailAsync(newUser.Email, "Confirm your email",
//                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
//
//        }

        [HttpGet]
        [Route("Api/users/login")]
        public async Task<CustomUser> SignIn([FromHeader(Name = "Email")] string email,
            [FromHeader(Name = "Password")] string password)
        {
            byte[] decodedEmail = Convert.FromBase64String(email);
            email = System.Text.Encoding.UTF8.GetString(decodedEmail);

            byte[] decodedPassword = Convert.FromBase64String(password);
            password = System.Text.Encoding.UTF8.GetString(decodedPassword);

            var user = _usersManager.GetUserByEmail(email);

            var result = await _signInManager.PasswordSignInAsync(user.UserName, password, true, true);
            if (result.Succeeded)
            {
                return user;
            }

            return null;
        }

        #endregion
    }
}