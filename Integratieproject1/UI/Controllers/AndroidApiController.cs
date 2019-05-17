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

//        private readonly UserManager<IdentityUser> _userManager;
//        private readonly IEmailSender _emailSender;
        private readonly JWTSettings _options;

        public AndroidApiController(
            SignInManager<CustomUser> signInManager,
//            UserManager<IdentityUser> userManager, 
//            EmailSender emailSender,
            IOptions<JWTSettings> optionsAccessor)
        {
            _ideationsManager = new IdeationsManager();
            _surveysManager = new SurveysManager();
            _projectsManager = new ProjectsManager();
            _usersManager = new UsersManager();
            _signInManager = signInManager;
//            _userManager = userManager;
//            _emailSender = emailSender;
            _options = optionsAccessor.Value;
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


        [HttpGet]
        [Route("Api/tags")]
        public IEnumerable<Tag> getTags()
        {
            return _ideationsManager.GetAllTags().ToList();
        }

        #region Users

        [HttpGet]
        [Route("Api/users")]
        public IEnumerable<IdentityUser> GetUsers()
        {
            return _usersManager.GetUsers("User");
        }

        [HttpPost]
        [Route("Api/users/{email}")]
        public async Task<IdentityUser> GetUser(string email, string password)
        {
//            IAuthenticationHandlerProvider authProvider = new AuthenticationHandlerProvider();
//            string auth = httpContext.Request.Headers["Authorization"];
//            HttpWebResponse response = new HttpWebResponse();
//            var auth = response.GetResponseHeader(HeaderNames.Authorization);
//            var auth = AuthenticationHeaderValue.Parse(HeaderNames.Authorization);
//            Console.WriteLine("==============================");
//            Console.WriteLine(auth);
//            Console.WriteLine("==============================");

            var result = await _signInManager.PasswordSignInAsync(email, password, true, true);

            return result.Succeeded ? _usersManager.GetUserByEmail(email) : null;
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
//                return new JsonResult(new Dictionary<string, object>
//                {
//                    {"access_token", GetAccessToken(email)},
//                    {"id_token", GetIdToken(user)}
//                });

                return user;
            }

            return null;
//            return new JsonResult("Unable to sign in") {StatusCode = 401};
        }

        //dXNlckBnbWFpbC5jb20=/VXNlcjEyMyE=
        [HttpGet]
        [Route("Api/users/login/{email}/{password}")]
        public async Task<IActionResult> TestSignIn(string email, string password)
        {
            byte[] decodedEmail = Convert.FromBase64String(email);
            email = System.Text.Encoding.UTF8.GetString(decodedEmail);

            byte[] decodedPassword = Convert.FromBase64String(password);
            password = System.Text.Encoding.UTF8.GetString(decodedPassword);
            
            Console.WriteLine(
                "============== Email: " + email + " ============= Password: " + password + " ============");
            
            var user = _usersManager.GetUserByEmail(email);
            
            Console.WriteLine("USER!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + user);
            
            var result = await _signInManager.PasswordSignInAsync(user.UserName, password, true, true);
            
            var json = new JsonResult("Unable to sign in") {StatusCode = 401};
            if (result.Succeeded)
            {
//                var user = _usersManager.GetUserByEmail(email);
                
                json = new JsonResult(new Dictionary<string, object>
                {
                    {"access_token", GetAccessToken(email)},
                    {"id_token", GetIdToken(user)}
                });
            }
            
            Console.WriteLine("================================");
            Console.WriteLine(json.Value.ToString());
            Console.WriteLine("================================");

            return json;
        }

        private string GetIdToken(IdentityUser user)
        {
            var payload = new Dictionary<string, object>
            {
                {"id", user.Id},
                {"sub", user.Email},
                {"email", user.Email},
                {"emailConfirmed", user.EmailConfirmed},
            };
            return GetToken(payload);
        }

        private string GetAccessToken(string email)
        {
            var payload = new Dictionary<string, object>
            {
                {"sub", email},
                {"email", email}
            };
            return GetToken(payload);
        }

        private string GetToken(Dictionary<string, object> payload)
        {
            var secret = _options.SecretKey;

            payload.Add("iss", _options.Issuer);
            payload.Add("aud", _options.Audience);
            payload.Add("nbf", ConvertToUnixTimestamp(DateTime.Now));
            payload.Add("iat", ConvertToUnixTimestamp(DateTime.Now));
            payload.Add("exp", ConvertToUnixTimestamp(DateTime.Now.AddDays(7)));
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            return encoder.Encode(payload, secret);
        }

        private JsonResult Errors(IdentityResult result)
        {
            var items = result.Errors
                .Select(x => x.Description)
                .ToArray();
            return new JsonResult(items) {StatusCode = 400};
        }

        private JsonResult Error(string message)
        {
            return new JsonResult(message) {StatusCode = 400};
        }

        private static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        #endregion
    }
}