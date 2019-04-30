using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Integratieproject1.BL.Managers;
using Integratieproject1.Domain.Projects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Integratieproject1.UI.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminController : Controller
    {
        private ProjectsManager projectsManager;
        private IdeationsManager ideationsManager;
        private SurveysManager surveysManager;
        private UsersManager usersManager;

        public SuperAdminController()
        {
            projectsManager = new ProjectsManager();
            usersManager = new UsersManager();
            ideationsManager = new IdeationsManager();
            surveysManager = new SurveysManager();
        }

        public IActionResult SuperAdmin(IdentityUser user)
        {
            return View("/UI/Views/SuperAdmin/SuperAdmin.cshtml", user);
        }

        public IActionResult Admins()
        {

            IList<IdentityUser> admins = usersManager.GetUsers("ADMIN");
            return View("/UI/Views/SuperAdmin/Admins.cshtml", admins);
        }
        
        public IActionResult DeleteAdmin(string adminId)
        {
            usersManager.DeleteUser(adminId);
            IList<IdentityUser> admins = usersManager.GetUsers("ADMIN");
            return View("/UI/Views/SuperAdmin/Admins.cshtml", admins);
        }
        
        public IActionResult DeleteAdminRole(string adminId)
        {
            usersManager.DeleteRole(adminId,"ADMIN");
            usersManager.GiveRole(adminId,"USER");
            IList<IdentityUser> admins = usersManager.GetUsers("ADMIN");
            return View("/UI/Views/SuperAdmin/Admins.cshtml", admins);
        }
        
        public IActionResult Users()
        {
            IList<IdentityUser> users = usersManager.GetUsers("USER");
            return View("/UI/Views/SuperAdmin/Users.cshtml", users);
        }
        
        public IActionResult DeleteUser(string userId)
        {
            usersManager.DeleteUser(userId);
            IList<IdentityUser> users = usersManager.GetUsers("USER");
            return View("/UI/Views/SuperAdmin/Users.cshtml", users);
        }
        
        public IActionResult GiveAdminRole(string userId)
        {
            usersManager.GiveRole(userId,"ADMIN");
            IList<IdentityUser> users = usersManager.GetUsers("USER");
            return View("/UI/Views/SuperAdmin/Users.cshtml", users);
        }
        
        public IActionResult AdminProjects(string adminId)
        {
            IList<AdminProject> adminProjects = projectsManager.GetAllAdminProjects(adminId).ToList();
            return View("/UI/Views/SuperAdmin/AdminProjects.cshtml", adminProjects);
        }
        
        public IActionResult DeleteAdminProject(int adminProjectId)
        {
            projectsManager.DeleteAdminProject(adminProjectId);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Projects()
        {
            IList<Project> projects = projectsManager.GetAllProjects();
            return View("/UI/Views/SuperAdmin/Projects.cshtml", projects);
        }
        
        public IActionResult DeleteProject(int projectId)
        {
            projectsManager.DeleteProject(projectId);
            IList<Project> projects = projectsManager.GetAllProjects();
            return View("/UI/Views/SuperAdmin/Projects.cshtml", projects);
        }
        
        public IActionResult CreateProject()
        {
            return View("/UI/Views/SuperAdmin/CreateProject.cshtml");
        }
        
        [HttpPost]
        public IActionResult CreateProject(Project project, int platformId)
        {
            
            if (ModelState.IsValid)
            {
                ClaimsPrincipal currentUser = User;
                string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                projectsManager.CreateProject(project, currentUserId,platformId);
            }

            IList<Project> projects = projectsManager.GetAllProjects();
            return View("/UI/Views/SuperAdmin/Projects.cshtml", projects);
        }
        
        public IActionResult EditProject(int projectId)
        {
            Project project = projectsManager.GetProject(projectId);
            return View("/UI/Views/SuperAdmin/EditProject.cshtml", project);
        }
        
        [HttpPost]
        public IActionResult EditProject(int projectId, Project project)
        {
            if (ModelState.IsValid)
            {
                projectsManager.EditProject(project, projectId);
            }
                
            IList<Project> projects = projectsManager.GetAllProjects();
            return View("/UI/Views/SuperAdmin/Projects.cshtml", projects);
        }

        public IActionResult Platforms()
        {
            IList<Platform> platforms = projectsManager.GetAllPlatforms();
            return View("/UI/Views/SuperAdmin/Platforms.cshtml", platforms);
        }

        public IActionResult DeletePlatform(int platformId)
        {
            projectsManager.DeletePlatform(platformId);
            IList<Platform> platforms = projectsManager.GetAllPlatforms();
            return View("/UI/Views/SuperAdmin/Platforms.cshtml", platforms);
        }
        
        public IActionResult EditPlatform(int platformId)
        {
            Platform platform = projectsManager.GetPlatform(platformId);
            return View("/UI/Views/SuperAdmin/EditPlatform.cshtml", platform);
        }
        
        [HttpPost]
        public IActionResult EditPlatform(int platformId, Platform platform)
        {
            if (ModelState.IsValid)
            {
                projectsManager.EditPlatform(platform, platformId);
            }
                
            IList<Platform> platforms = projectsManager.GetAllPlatforms();
            return View("/UI/Views/SuperAdmin/Platforms.cshtml", platforms);
        }

        public IActionResult CreatePlatform()
        {
            return View("/UI/Views/SuperAdmin/CreatePlatform.cshtml");
        }

        [HttpPost]
        public IActionResult CreatePlatform(Platform platform)
        {

            if (ModelState.IsValid)
            {
                projectsManager.CreatePlatform(platform);
            }

            IList<Platform> platforms = projectsManager.GetAllPlatforms();
            return View("/UI/Views/SuperAdmin/Platforms.cshtml", platforms);
        }

        public IActionResult AddAdminsToProject(int projectId)
        {
            IList<IdentityUser> admins = usersManager.GetUsers("ADMIN");
            ViewBag.ProjectId = projectId;
            return View("/UI/Views/SuperAdmin/AddAdminsToProject.cshtml", admins);
        }

        public IActionResult AddAdminProjects(int projectId, string adminId)
        {
            projectsManager.CreateAdminProject(projectId, adminId);
            IList<IdentityUser> admins = usersManager.GetUsers("ADMIN");
            ViewBag.ProjectId = projectId;
            return View("/UI/Views/SuperAdmin/AddAdminsToProject.cshtml", admins);
        }
    }
}