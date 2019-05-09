using System.Collections.Generic;
using Integratieproject1.BL.Managers;
using Integratieproject1.DAL.Repositories;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Projects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Integratieproject1.UI.Controllers
{
    [Authorize(Roles = "Mod, Admin, SuperAdmin ")]
    public class ModeratorController : Controller
    {
        private readonly ProjectsManager _projectsManager;
        private readonly IdeationsManager _ideationsManager;
        private readonly SurveysManager _surveysManager;
        private readonly UsersManager _usersManager;

        public ModeratorController()
        {
            _projectsManager = new ProjectsManager();
            _usersManager = new UsersManager();
            _ideationsManager = new IdeationsManager();
            _surveysManager = new SurveysManager();
        }


        public IActionResult Index(IdentityUser user)
        {
            return View("/UI/Views/Moderator/Index.cshtml", user);
        }

        public IActionResult Projects()
        {
            IList<Project> projects = _projectsManager.GetAllProjects();
            return View("/UI/Views/Moderator/Projects.cshtml", projects);
        }

        public IActionResult Posts(int projectId)
        {
            ViewBag.ReportedIdeas = _ideationsManager.GetReportedIdeas(projectId);
            ViewBag.ReportedReactions = _ideationsManager.GetReportedReactions(projectId);
            Project project = _projectsManager.GetProject(projectId);
            return View("/UI/Views/Moderator/Posts.cshtml", project);
        }

        public IActionResult PostCorrect(int projectId, int id, string type)
        {
            _ideationsManager.PostCorrect(id, type);
            Project project = _projectsManager.GetProject(projectId);
            return View("/UI/Views/Moderator/Posts.cshtml", project);
        }

        public IActionResult DeletePost(int projectId, int id, string type)
        {
            _ideationsManager.DeletePost(id, type);
                        Project project = _projectsManager.GetProject(projectId);
                        return View("/UI/Views/Moderator/Posts.cshtml", project);
        }

        public IActionResult Users()
        {
            UsersManager usersManager = new UsersManager();
            IList<IdentityUser> users = usersManager.GetUsers("USER");
            return View("/UI/Views/Moderator/Users.cshtml", users);
        }

        public IActionResult BlockAccount(string userId, int days)
        {
            UsersManager usersManager = new UsersManager();
            usersManager.BlockUser(userId, days);
            IList<IdentityUser> users = usersManager.GetUsers("USER");
            return View("/UI/Views/Moderator/Users.cshtml", users);
        }

    }
}