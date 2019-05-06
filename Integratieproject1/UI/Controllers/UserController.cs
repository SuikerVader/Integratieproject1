using System.Security.Claims;
using Integratieproject1.BL.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Integratieproject1.Domain.Users;

namespace Integratieproject1.UI.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly ProjectsManager _projectsManager;
        private readonly IdeationsManager _ideationsManager;
        private readonly SurveysManager _surveysManager;
        private readonly UsersManager _usersManager;

        public UserController()
        {
            _projectsManager = new ProjectsManager();
            _usersManager = new UsersManager();
            _ideationsManager = new IdeationsManager();
            _surveysManager = new SurveysManager();
        }

        IActionResult CreateVerificationRequest(string req)
        {
            ClaimsPrincipal currentUser = ClaimsPrincipal.Current;
            string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            IdentityUser user = _usersManager.GetUser(currentUserId);
            _usersManager.CreateVerificationRequest(_usersManager.GetUser(ClaimTypes.NameIdentifier), req);
            return View("/UI/Views/User/Index.cshtml");
        }
    }
}