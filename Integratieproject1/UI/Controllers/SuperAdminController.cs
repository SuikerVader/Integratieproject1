using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Integratieproject1.BL.Managers;
using Integratieproject1.Domain.Projects;
using Integratieproject1.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Integratieproject1.UI.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminController : Controller
    {
        private ProjectsManager _projectsManager;
        private IdeationsManager _ideationsManager;
        private SurveysManager _surveysManager;
        private UsersManager _usersManager;

        public SuperAdminController()
        {
            _projectsManager = new ProjectsManager();
            _usersManager = new UsersManager();
            _ideationsManager = new IdeationsManager();
            _surveysManager = new SurveysManager();
        }

        public IActionResult SuperAdmin(IdentityUser user)
        {
            return View("/UI/Views/SuperAdmin/SuperAdmin.cshtml", user);
        }

        public IActionResult Admins(string sortOrder, string searchString)
        {
            IEnumerable<CustomUser> admins = _usersManager.GetUsersBySort("ADMIN", sortOrder);
            ViewData["UserNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "username_desc" : "";
            ViewData["SurnameSortParm"] = sortOrder == "Surname" ? "surname_desc" : "Surname";
            ViewData["NameSortParm"] = sortOrder == "Name" ? "name_desc" : "Name";
            ViewData["EmailSortParm"] = sortOrder == "Email" ? "email_desc" : "Email";
            ViewData["AgeSortParm"] = sortOrder == "Age" ? "age_desc" : "Age";
            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                admins = admins.Where(u => u.UserName.ToLower().Contains(searchString)
                                       || u.Surname.ToLower().Contains(searchString)
                                       || u.Name.ToLower().Contains(searchString)
                                       || u.Email.ToLower().Contains(searchString));
            }
            return View("/UI/Views/SuperAdmin/Admins.cshtml", admins.ToList());
        }
        
        public IActionResult DeleteAdmin(string adminId)
        {
            _usersManager.DeleteUser(adminId);
            IList<CustomUser> admins = _usersManager.GetUsers("ADMIN");
            return View("/UI/Views/SuperAdmin/Admins.cshtml", admins);
        }
        
        public IActionResult DeleteAdminRole(string adminId)
        {
            _usersManager.DeleteRole(adminId,"ADMIN");
            _usersManager.GiveRole(adminId,"USER");
            IList<CustomUser> admins = _usersManager.GetUsers("ADMIN");
            return View("/UI/Views/SuperAdmin/Admins.cshtml", admins);
        }
        
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
            return View("/UI/Views/SuperAdmin/Users.cshtml", users.ToList());
        }
        
        public IActionResult DeleteUser(string userId)
        {
            _usersManager.DeleteUser(userId);
            IList<CustomUser> users = _usersManager.GetUsers("USER");
            return View("/UI/Views/SuperAdmin/Users.cshtml", users);
        }
        
        public IActionResult GiveAdminRole(string userId)
        {
            _usersManager.GiveRole(userId,"ADMIN");
            IList<CustomUser> users = _usersManager.GetUsers("USER");
            return View("/UI/Views/SuperAdmin/Users.cshtml", users);
        }
        
        public IActionResult AdminProjects(string adminId, string sortOrder, string searchString)
        {
            IEnumerable<AdminProject> adminProjects = _projectsManager.GetAllAdminProjectsBySort(adminId, sortOrder).ToList();
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["StatusSortParm"] = sortOrder == "Status" ? "status_desc" : "Status";
            ViewData["StartDateSortParm"] = sortOrder == "StartDate" ? "startdate_desc" : "StartDate";
            ViewData["EndDateSortParm"] = sortOrder == "EndDate" ? "enddate_desc" : "EndDate";
            ViewData["PlatformSortParm"] = sortOrder == "Platform" ? "platform_desc" : "Platform";
            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                adminProjects = adminProjects.Where(p => p.Project.ProjectName.ToLower().Contains(searchString)
                                       || p.Project.Status.ToLower().Contains(searchString)
                                       || p.Project.Platform.PlatformName.ToLower().Contains(searchString));
            }
            return View("/UI/Views/SuperAdmin/AdminProjects.cshtml", adminProjects.ToList());
        }
        
        public IActionResult DeleteAdminProject(int adminProjectId)
        {
            _projectsManager.DeleteAdminProject(adminProjectId);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Projects(string sortOrder, string searchString)
        {
            IEnumerable<Project> projects = _projectsManager.GetAllProjectsBySort(sortOrder);
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
            return View("/UI/Views/SuperAdmin/Projects.cshtml", projects.ToList());
        }
        
        public IActionResult DeleteProject(int projectId)
        {
            _projectsManager.DeleteProject(projectId);
            IList<Project> projects = _projectsManager.GetAllProjects();
            return View("/UI/Views/SuperAdmin/Projects.cshtml", projects);
        }
        
        public IActionResult CreateProject()
        {
            return View("/UI/Views/SuperAdmin/CreateProject.cshtml");
        }
        
        [HttpPost]
        public IActionResult CreateProject(Project project, int platformId, IFormFile formFile)
        {
            
            if (ModelState.IsValid)
            {
                if (formFile != null)
                {
                    project.BackgroundImage = GetImagePath(formFile); 
                }
                ClaimsPrincipal currentUser = User;
                string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                _projectsManager.CreateProject(project, currentUserId,platformId);
            }

            IList<Project> projects = _projectsManager.GetAllProjects();
            return View("/UI/Views/SuperAdmin/Projects.cshtml", projects);
        }
        
        public IActionResult EditProject(int projectId)
        {
            Project project = _projectsManager.GetProject(projectId);
            return View("/UI/Views/SuperAdmin/EditProject.cshtml", project);
        }
        
        [HttpPost]
        public IActionResult EditProject(int projectId, Project project, IFormFile formFile)
        {
            if (ModelState.IsValid)
            {
                if (formFile != null)
                {
                    project.BackgroundImage = GetImagePath(formFile); 
                }
                _projectsManager.EditProject(project, projectId);
            }
                
            IList<Project> projects = _projectsManager.GetAllProjects();
            return View("/UI/Views/SuperAdmin/Projects.cshtml", projects);
        }
        private string GetImagePath(IFormFile file)
        {
            string wwwroot = "wwwroot/";
            string uploads = "/images/uploads/";
            string path = wwwroot + uploads;

            if (file.Length > 0)
            {
                string imagePath = Guid.NewGuid() + Path.GetExtension(file.FileName);

                using (var fileStream = new FileStream(Path.Combine(path, imagePath), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                return Path.Combine(uploads, imagePath);
            }

            return null;
        }
        public IActionResult DeleteBackgroundImageProject(int projectId)
        {
            _projectsManager.DeleteBackgroundImageProject(projectId);
            Project returnProject = _projectsManager.GetProject(projectId);
            return View("/UI/Views/SuperAdmin/EditProject.cshtml", returnProject);
        }

        public IActionResult Platforms(string sortOrder, string searchString)
        {
            IEnumerable<Platform> platforms = _projectsManager.GetAllPlatformsBySort(sortOrder);
            ViewData["IdSortParm"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewData["NameSortParm"] = sortOrder == "Name" ? "name_desc" : "Name";
            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                platforms = platforms.Where(p => p.PlatformName.ToLower().Contains(searchString));
            }
            return View("/UI/Views/SuperAdmin/Platforms.cshtml", platforms.ToList());
        }

        public IActionResult DeletePlatform(int platformId)
        {
            _projectsManager.DeletePlatform(platformId);
            IList<Platform> platforms = _projectsManager.GetAllPlatforms();
            return View("/UI/Views/SuperAdmin/Platforms.cshtml", platforms);
        }
        
        public IActionResult EditPlatform(int platformId)
        {
            Platform platform = _projectsManager.GetPlatform(platformId);
            return View("/UI/Views/SuperAdmin/EditPlatform.cshtml", platform);
        }
        
        [HttpPost]
        public IActionResult EditPlatform(int platformId, Platform platform,IFormFile formFile)
        {
            if (ModelState.IsValid)
            {
                if (formFile != null)
                {
                    platform.BackgroundImage = GetImagePath(formFile);
                                    
                }
               _projectsManager.EditPlatform(platform, platformId);
            }
                
            IList<Platform> platforms = _projectsManager.GetAllPlatforms();
            return View("/UI/Views/SuperAdmin/Platforms.cshtml", platforms);
        }
        public IActionResult DeleteBackgroundImagePlatform(int platformId)
        {
            _projectsManager.DeleteBackgroundImagePlatform(platformId);
            Platform platform = _projectsManager.GetPlatform(platformId);
            return View("/UI/Views/SuperAdmin/EditPlatform.cshtml", platform);
        }

        public IActionResult DeleteLogoPlatform(int platformId)
        {
            _projectsManager.DeleteLogoPlatform(platformId);
            Platform platform = _projectsManager.GetPlatform(platformId);
            return View("/UI/Views/SuperAdmin/EditPlatform.cshtml", platform);
        }

        public IActionResult CreatePlatform()
        {
            return View("/UI/Views/SuperAdmin/CreatePlatform.cshtml");
        }

        [HttpPost]
        public IActionResult CreatePlatform(Platform platform, IFormFile formFile, IFormFile logoFile)
        {

            if (ModelState.IsValid)
            {
                if (formFile != null)
                {
                    platform.BackgroundImage = GetImagePath(formFile);
                                    
                }
                if (logoFile != null)
                {
                    platform.Logo = GetImagePath(logoFile);
                                    
                }
                _projectsManager.CreatePlatform(platform);
            }

            IList<Platform> platforms = _projectsManager.GetAllPlatforms();
            return View("/UI/Views/SuperAdmin/Platforms.cshtml", platforms);
        }

        public IActionResult AddAdminsToProject(int projectId, string sortOrder, string searchString)
        {
            IEnumerable<CustomUser> admins = _projectsManager.GetNotProjectAdminsBySort(projectId, sortOrder);
            ViewData["UserNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "username_desc" : "";
            ViewData["SurnameSortParm"] = sortOrder == "Surname" ? "surname_desc" : "Surname";
            ViewData["NameSortParm"] = sortOrder == "Name" ? "name_desc" : "Name";
            ViewData["EmailSortParm"] = sortOrder == "Email" ? "email_desc" : "Email";
            ViewData["AgeSortParm"] = sortOrder == "Age" ? "age_desc" : "Age";
            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                admins = admins.Where(u => u.UserName.ToLower().Contains(searchString)
                                       || u.Surname.ToLower().Contains(searchString)
                                       || u.Name.ToLower().Contains(searchString)
                                       || u.Email.ToLower().Contains(searchString));
            }
            ViewBag.ProjectId = projectId;
            return View("/UI/Views/SuperAdmin/AddAdminsToProject.cshtml", admins.ToList());
        }

        public IActionResult AddAdminProjects(int projectId, string adminId)
        {
            _projectsManager.CreateAdminProject(projectId, adminId);
            IList<CustomUser> admins = _projectsManager.GetNotProjectAdmins(projectId);
            ViewBag.ProjectId = projectId;
            return View("/UI/Views/SuperAdmin/AddAdminsToProject.cshtml", admins);
        }

        public IActionResult EditLayout(int platformId)
        {
            Platform platform = _projectsManager.GetPlatform(platformId);
            return View("/UI/Views/SuperAdmin/EditLayout.cshtml", platform);
        }

        [HttpPost]
        public IActionResult EditLayout(int platformId, Platform platform)
        {
            if (ModelState.IsValid)
            {
                _projectsManager.EditPlatformLayout(platform, platformId);
            }

            IList<Platform> platforms = _projectsManager.GetAllPlatforms();
            return View("/UI/Views/SuperAdmin/Platforms.cshtml", platforms);
        }

    }
}