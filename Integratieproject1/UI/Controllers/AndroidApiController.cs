using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Integratieproject1.BL.Managers;
using Integratieproject1.Domain.Datatypes;
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
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
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
        
        //Work in progress
        [HttpPost]
        [Route("Api/idea")]
        public Idea GetIdea([FromHeader] int ideationid, [FromHeader] string userid, [FromBody] JsonObject<IdeaObject> parameters)
        {
            
            Console.WriteLine(parameters.ToString());

            return null;
            //return _ideationsManager.PostIdea();
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
        [Route("Api/sharedVotes/{id}")]
        public IEnumerable<Vote> GetVotes(int id)
        {
            return _ideationsManager.GetIdeaVote(id);
        }

        [HttpPost]
        [Route("Api/like")]
        public void postLike([FromHeader] int reactionId,[FromHeader] string userid)
        {
            Console.WriteLine(reactionId + userid);
            _ideationsManager.postLike(reactionId, userid);
        }
        
        
        [HttpGet]
        [Route("Api/like/{id}")]
        public IEnumerable<Like> GetLikes(int id)
        {
            return null;
        }


        [HttpPost]
        [Route("Api/Reaction")]
        public void postReaction([FromHeader] String param,[FromHeader] int id,[FromHeader] String userId,[FromHeader] String element)
        {
             ArrayList parameter = new ArrayList();
             parameter.Add(param);
            _ideationsManager.PostReaction(parameters:parameter,id:id,userId:userId,element:element);
        }

        #endregion

        #region Projects

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
        //public void AndroidVote([FromBody] int id)
        public void androidVote([FromHeader] int id, [FromHeader] String vote, [FromHeader] string userId)
        {
            Console.WriteLine(id);
            Console.WriteLine(userId);
            Console.WriteLine(vote);
            VoteType voteType = (VoteType) Enum.Parse(typeof(VoteType), vote, true);
            _ideationsManager.CreateVote(id,voteType,userId);
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

        [HttpGet]
        [Route("Api/users/login")]
        public async Task<CustomUser> SignIn([FromHeader(Name = "Username")] string username,
            [FromHeader(Name = "Password")] string password)
        {
            byte[] decodedUsername = Convert.FromBase64String(username);
            username = System.Text.Encoding.UTF8.GetString(decodedUsername);

            byte[] decodedPassword = Convert.FromBase64String(password);
            password = System.Text.Encoding.UTF8.GetString(decodedPassword);

            CustomUser user = null;
            
            if (username.Contains("@"))
            {
                user = _usersManager.GetUserByEmail(username);
            }
            else
            {
                user = _usersManager.GetUserByUsername(username);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, password, true, true);
            if (result.Succeeded)
            {
                return user;
            }

            return null;
        }

        [HttpPost]
        [Route("Api/users/update")]
        public void UpdateUser([FromHeader(Name = "Username")] string username, [FromBody] UserUpdateValuesModel userUpdateValues)
        {
            var user = _usersManager.GetUserByUsername(username);

            if (user != null)
            {
                user.Surname = userUpdateValues.Surname;
                user.Name = userUpdateValues.LastName;
                user.Sex = userUpdateValues.Sex;
                user.Age = Int32.Parse(userUpdateValues.Age);
                user.Zipcode = userUpdateValues.ZipCode;
            
                _usersManager.UpdateUser(user);
            }
        }

        #endregion
    }
}