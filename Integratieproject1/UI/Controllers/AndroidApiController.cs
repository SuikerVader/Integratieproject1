using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Integratieproject1.Areas.Identity.Services;
using Integratieproject1.BL.Managers;
using Integratieproject1.Domain.Projects;
using Microsoft.AspNetCore.Mvc;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Surveys;
using Integratieproject1.Domain.Users;
using Integratieproject1.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
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

//        [HttpGet]
//        [Route("Api/answers/{id}")]
//        public IEnumerable<Answer> GetAnswers()
//        {
//            return null;
//        }

        [HttpGet]
        [Route("Api/questions/{id}")]
        public IEnumerable<Question> GetQuestions(int id)
        {
            return _surveysManager.GetQuestions(id).ToList();
        }
        
        [HttpPost]
        [Route("Api/questions/answers/save")]
        public void PostAnswers([FromHeader(Name = "SurveyId")] int surveyId, [FromBody] AnswerPostValuesModel[] answerPostValuesArray)
        {
            var answers = new ArrayList();
            
            foreach (var answerPostValues in answerPostValuesArray)
            {
                byte[] decodedAnswerText = Convert.FromBase64String(answerPostValues.AnswerText.Replace("%3D", "="));
                answerPostValues.AnswerText = System.Text.Encoding.UTF8.GetString(decodedAnswerText);
                
                answers.Add(answerPostValues.AnswerText);
            }
            
            _surveysManager.UpdateAnswers(answers, surveyId);
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

        [HttpGet]
        [Route("Api/users/register")]
        public async Task<CustomUser> Register([FromHeader(Name = "Username")] string username,
            [FromHeader(Name = "Email")] string email,
            [FromHeader(Name = "Password")] string password)
        {
            byte[] decodedUsername = Convert.FromBase64String(username);
            username = System.Text.Encoding.UTF8.GetString(decodedUsername);
            
            byte[] decodedEmail = Convert.FromBase64String(email);
            email = System.Text.Encoding.UTF8.GetString(decodedEmail);

            byte[] decodedPassword = Convert.FromBase64String(password);
            password = System.Text.Encoding.UTF8.GetString(decodedPassword);

            if (_usersManager.GetUserByEmail(username) == null && _usersManager.GetUserByUsername(username) == null)
            {
                var user = new CustomUser
                {
                    UserName = username,
                    Email = email
                };
                
                var result = await _signInManager.UserManager.CreateAsync(user, password);
                _usersManager.GiveRole(user.Id, "USER");

                if (result.Succeeded)
                {
                    return user;
                }
            }

            return null;
        }

        #endregion
    }
}