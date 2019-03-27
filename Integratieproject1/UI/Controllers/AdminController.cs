using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Integratieproject1.BL.Managers;
using Integratieproject1.Domain;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Projects;
using Integratieproject1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Integratieproject1.UI.Controllers
{
    public class AdminController : Controller
    {
        private ProjectsManager projectsManager;
        private IdeationsManager ideationsManager;
        private SurveysManager surveysManager;
        private UsersManager usersManager;
        
        public AdminController()
        {
            projectsManager = new ProjectsManager();
            usersManager = new UsersManager();
            ideationsManager = new IdeationsManager();
            surveysManager = new SurveysManager();
        }
        public IActionResult Admin(int userId)
        {
            LoggedInUser user = usersManager.GetLoggedInUser(userId);
            return View("/UI/Views/Admin/Admin.cshtml", user );
        }
        public IActionResult Projects(int userId)
        {
            IList<Project> projects = projectsManager.GetAdminProjects(userId);            
            return View("/UI/Views/Admin/Projects.cshtml" , projects);
        }

        public IActionResult Phases(int projectId)
        {
            IList<Phase> phases = projectsManager.GetPhases(projectId);
            ViewData["ProjectId"] = projectId;
            return View("/UI/Views/Admin/Phases.cshtml" , phases);
        }
        public IActionResult EditProject(int projectId)
        {
            Project project = projectsManager.GetProject(projectId);
            return View("/UI/Views/Admin/EditProject.cshtml", project);
        }

        [HttpPost]
        public IActionResult EditProject(int projectId,Project project)
        {
            if (ModelState.IsValid)
            {
                projectsManager.EditProject(project, projectId);
                return RedirectToAction("Index","Home");
            }

            return RedirectToAction("Index","Home");
        }

        public IActionResult CreateProject(int userId)
        {
            ViewData["UserId"] = userId;
            return View("/UI/Views/Admin/CreateProject.cshtml");
        }
        
        [HttpPost]
        public IActionResult CreateProject(Project project, int userId)
        {
            if (ModelState.IsValid)
            {
               projectsManager.CreateProject(project, userId); 
               return RedirectToAction("Index","Home");
            }
            return RedirectToAction("Index","Home");
        }

        public IActionResult DeleteProject(int projectId)
        {
            projectsManager.DeleteProject(projectId);
            return RedirectToAction("Index", "Home");
        }

        

        public IActionResult EditPhase(int phaseId)
        {
            Phase phase = projectsManager.GetPhase(phaseId);
            return View("/UI/Views/Admin/EditPhase.cshtml", phase);
        }
        [HttpPost]
        public IActionResult EditPhase(int phaseId, Phase phase)
        {
            
            if (ModelState.IsValid)
            {
                Phase editedPhase = projectsManager.EditPhase(phase, phaseId);
                
                return RedirectToAction("Index","Home");
            }

            return RedirectToAction("Index","Home");
        }
        
        public IActionResult AddPhase(int projectId)
        {
            Phase phase = new Phase();
            Project project = projectsManager.GetProject(projectId);
            phase.Project = project;
            if (project.Phases !=null && project.Phases.Count > 0)
            {
                phase.PhaseNr = project.Phases.Last().PhaseNr + 1;
                phase.StartDate = project.Phases.Last().EndDate;
            }
            else
            {
                phase.PhaseNr = 1;
                phase.StartDate = project.StartDate;
            }            
            
            phase.EndDate = phase.StartDate;
            return View("/UI/Views/Admin/CreatePhase.cshtml", phase);
        }
        [HttpPost]
        public IActionResult AddPhase(Phase phase, int phaseNr, int projectId)
        {
            phase.PhaseNr = phaseNr;
            phase.Project = projectsManager.GetProject(projectId);
            if (ModelState.IsValid)
            {
                Phase createdPhase = projectsManager.CreatePhase(phase);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index","Home");
        }

        public IActionResult DeletePhase(int phaseId)
        {
            projectsManager.DeletePhase(phaseId);
            return RedirectToAction("Index","Home");
        }
    }
}