using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Integratieproject1.BL.Managers;
using Integratieproject1.DAL.Repositories;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Projects;
using Integratieproject1.Domain.Users;
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

        public IActionResult Projects(string sortOrder, string searchString)
        {
            ClaimsPrincipal currentUser = User;
            string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            IEnumerable<Project> projects = _projectsManager.GetAdminProjectsBySort(currentUserId, sortOrder);
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["StatusSortParm"] = sortOrder == "Status" ? "status_desc" : "Status";
            ViewData["StartDateSortParm"] = sortOrder == "StartDate" ? "startdate_desc" : "StartDate";
            ViewData["EndDateSortParm"] = sortOrder == "EndDate" ? "enddate_desc" : "EndDate";
            ViewData["PlatformSortParm"] = sortOrder == "Platform" ? "platform_desc" : "Platform";
            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                projects = projects.Where(p => p.ProjectName.ToLower().Contains(searchString)
                                       || p.Status.ToLower().Contains(searchString)
                                       || p.Platform.PlatformName.ToLower().Contains(searchString));
            }

            return View("/UI/Views/Moderator/Projects.cshtml", projects.ToList());
        }


        #region Posts
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

        #endregion

        #region Users

        public IActionResult Users(string sortOrder, string searchString)
        {
            IEnumerable<CustomUser> users = _usersManager.GetUsersBySort("USER", sortOrder);
            ViewData["UserNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "username_desc" : "";
            ViewData["SurnameSortParm"] = sortOrder == "Surname" ? "surname_desc" : "Surname";
            ViewData["NameSortParm"] = sortOrder == "Name" ? "name_desc" : "Name";
            ViewData["EmailSortParm"] = sortOrder == "Email" ? "email_desc" : "Email";
            ViewData["AgeSortParm"] = sortOrder == "Age" ? "age_desc" : "Age";
            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                users = users.Where(u => u.UserName.ToLower().Contains(searchString)
                                         || u.Surname.ToLower().Contains(searchString)
                                         || u.Name.ToLower().Contains(searchString)
                                         || u.Email.ToLower().Contains(searchString));
            }
            return View("/UI/Views/Moderator/Users.cshtml", users.ToList());
        }

        public IActionResult BlockAccount(string userId, int days)
        {
            UsersManager usersManager = new UsersManager();
            usersManager.BlockUser(userId, days);
            IList<CustomUser> users = usersManager.GetUsers("USER");
            return View("/UI/Views/Moderator/Users.cshtml", users);
        }

        #endregion

        #region Ideas

        public IActionResult Ideas(string sortOrder, string searchString)
        {
            IEnumerable<Idea> ideas = _ideationsManager.GetAllNonPublishedIdeas(sortOrder);
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["UserSortParm"] = sortOrder == "User" ? "user_desc" : "User";
            ViewData["IdeationSortParm"] = sortOrder == "Ideation" ? "ideation_desc" : "Ideation";
            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                ideas = ideas.Where(i => i.Title.ToLower().Contains(searchString)
                                         || i.IdentityUser.UserName.ToLower().Contains(searchString)
                                         || i.Ideation.CentralQuestion.ToLower().Contains(searchString));
            }
            return View("/UI/Views/Moderator/Ideas.cshtml", ideas.ToList());
        }

        public IActionResult Publish(int ideaId)
        {
            _ideationsManager.PublishIdea(ideaId);
            IList<Idea> ideas = _ideationsManager.GetAllNonPublishedIdeas("").ToList();
            return View("/UI/Views/Moderator/Ideas.cshtml", ideas.ToList());
        }


        #endregion
        

        
    }
}