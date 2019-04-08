using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using Integratieproject1.BL.Managers;
using Integratieproject1.DAL;
using Integratieproject1.Domain;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Projects;
using Integratieproject1.Domain.Surveys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Integratieproject1.UI.Controllers{}

    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        private readonly ProjectsManager _projectsManager;
        private readonly IdeationsManager _ideationsManager;
        private readonly SurveysManager _surveysManager;
        private readonly UsersManager _usersManager;

        public AdminController()
        {
            _projectsManager = new ProjectsManager();
            _usersManager = new UsersManager();
            _ideationsManager = new IdeationsManager();
            _surveysManager = new SurveysManager();
        }

        public IActionResult Admin()
        {
            ClaimsPrincipal currentUser = User;
            string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            IdentityUser user = _usersManager.GetUser(currentUserId);
            return View("/UI/Views/Admin/Admin.cshtml", user);
        }

        #region Project

        public IActionResult Projects()
        {
            ClaimsPrincipal currentUser = User;
            string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            IList<Project> projects = _projectsManager.GetAdminProjects(currentUserId);
            return View("/UI/Views/Admin/Projects.cshtml", projects);
        }

        public IActionResult EditProject(int projectId)
        {
            Project project = _projectsManager.GetProject(projectId);
            return View("/UI/Views/Admin/EditProject.cshtml", project);
        }

        [HttpPost]
        public IActionResult EditProject(int projectId, Project project)
        {
            if (ModelState.IsValid)
            {
                _projectsManager.EditProject(project, projectId);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult CreateProject(int userId)
        {
            ViewData["UserId"] = userId;
            return View("/UI/Views/Admin/CreateProject.cshtml");
        }

        [HttpPost]
        public IActionResult CreateProject(Project project)
        {
            if (ModelState.IsValid)
            {
                ClaimsPrincipal currentUser = User;
                string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                _projectsManager.CreateProject(project, currentUserId);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult DeleteProject(int projectId)
        {
            _projectsManager.DeleteProject(projectId);
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Phase

        public IActionResult Phases(int projectId)
        {
            IList<Phase> phases = _projectsManager.GetPhases(projectId);
            ViewData["ProjectId"] = projectId;
            return View("/UI/Views/Admin/Phases.cshtml", phases);
        }

        public IActionResult EditPhase(int phaseId)
        {
            Phase phase = _projectsManager.GetPhase(phaseId);
            return View("/UI/Views/Admin/EditPhase.cshtml", phase);
        }

        [HttpPost]
        public IActionResult EditPhase(int phaseId, Phase phase)
        {
            if (ModelState.IsValid)
            {
                Phase editedPhase = _projectsManager.EditPhase(phase, phaseId);

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AddPhase(int projectId)
        {
            Phase phase = _projectsManager.GetNewPhase(projectId);

            return View("/UI/Views/Admin/CreatePhase.cshtml", phase);
        }

        [HttpPost]
        public IActionResult AddPhase(Phase phase, int phaseNr, int projectId)
        {
            if (ModelState.IsValid)
            {
                Phase createdPhase = _projectsManager.CreatePhase(phase, phaseNr, projectId);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult DeletePhase(int phaseId)
        {
            _projectsManager.DeletePhase(phaseId);
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Ideation

        public IActionResult Ideations(int phaseId)
        {
            IList<Ideation> ideations = _ideationsManager.GetIdeations(phaseId);
            ViewData["PhaseId"] = phaseId;
            return View("/UI/Views/Admin/Ideations.cshtml", ideations);
        }

        public IActionResult EditIdeation(int ideationId)
        {
            Ideation ideation = _ideationsManager.GetIdeation(ideationId);
            return View("/UI/Views/Admin/EditIdeation.cshtml", ideation);
        }

        [HttpPost]
        public IActionResult EditIdeation(Ideation ideation, int ideationId)
        {
            if (ModelState.IsValid)
            {
                Ideation editIdeation = _ideationsManager.EditIdeation(ideation, ideationId);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AddIdeation(int phaseId)
        {
            ViewData["PhaseId"] = phaseId;
            return View("/UI/Views/Admin/CreateIdeation.cshtml");
        }

        [HttpPost]
        public IActionResult AddIdeation(Ideation ideation, int phaseId)
        {
            if (ModelState.IsValid)
            {
                _ideationsManager.CreateIdeation(ideation, phaseId);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult DeleteIdeation(int ideationId)
        {
            _ideationsManager.DeleteIdeation(ideationId);
            return RedirectToAction("Index", "Home");
        }

        #endregion


        /*public IActionResult Ideas(int ideationId)
        {
            IList<Idea> ideas = ideationsManager.GetIdeas(ideationId);
            ViewData["IdeationId"] = ideationId;
            return View("/UI/Views/Admin/Ideations.cshtml", ideas);
        }*/

        #region Survey

        public IActionResult Surveys(int phaseId)
        {
            IList<Survey> surveys = _surveysManager.GetSurveys(phaseId);
            ViewData["PhaseId"] = phaseId;
            return View("/UI/Views/Admin/Surveys.cshtml", surveys);
        }

        public IActionResult AddSurvey(int phaseId)
        {
            _surveysManager.CreateNewSurvey(phaseId);
            IList<Survey> surveys = _surveysManager.GetSurveys(phaseId);
            return View("/UI/Views/Admin/Surveys.cshtml", surveys);
        }

        public IActionResult EditSurvey(int surveyId)
        {
            Survey survey = _surveysManager.GetSurvey(surveyId);
            return View("/UI/Views/Admin/EditSurvey.cshtml", survey);
        }


        [HttpPost]
        public IActionResult EditSurvey(Survey survey, int surveyId)
        {
            if (ModelState.IsValid)
            {
                _surveysManager.EditSurvey(survey, surveyId);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult DeleteSurvey(int surveyId)
        {
            _surveysManager.DeleteSurvey(surveyId);
            return RedirectToAction("Index", "Home");
        }

        /*public IActionResult AddQuestion()
        {
            return PartialView("/UI/Views/Admin/_CreateQuestionPartial.cshtml");
        }*/

        [HttpPost]
        public IActionResult AddQuestion(Question question, int surveyId)
        {
            _surveysManager.CreateQuestion(question, surveyId);
            Survey survey = _surveysManager.GetSurvey(surveyId);
            return View("/UI/Views/Admin/EditSurvey.cshtml", survey);
        }
        [HttpPost]
        public IActionResult EditQuestion(Question question, int questionId, int surveyId)
        {
            _surveysManager.EditQuestion(question, questionId, surveyId);
            Survey survey = _surveysManager.GetSurvey(surveyId);
            return View("/UI/Views/Admin/EditSurvey.cshtml", survey);
        }
        
        public IActionResult DeleteQuestion(int questionId, int surveyId)
        {
           _surveysManager.DeleteQuestion(questionId); 
           Survey survey = _surveysManager.GetSurvey(surveyId);
           return View("/UI/Views/Admin/EditSurvey.cshtml", survey);
        }
        
        [HttpPost]
        public IActionResult EditAnswer(Answer answer, int answerId, int questionId, int surveyId)
        {
            _surveysManager.EditAnswer(answer, answerId, questionId);
            Survey survey = _surveysManager.GetSurvey(surveyId);
            return View("/UI/Views/Admin/EditSurvey.cshtml", survey);
            
        }
        [HttpPost]
        public IActionResult AddAnswer(Answer answer, int questionId, int surveyId)
        {
            answer.Question = _surveysManager.GetQuestion(questionId);
            _surveysManager.CreateAnswer(answer);
            Survey survey = _surveysManager.GetSurvey(surveyId);
            return View("/UI/Views/Admin/EditSurvey.cshtml", survey);
        }
        
        public IActionResult DeleteAnswer(int answerId, int surveyId)
        {
            _surveysManager.DeleteAnswer(answerId);
            Survey survey = _surveysManager.GetSurvey(surveyId);
            return View("/UI/Views/Admin/EditSurvey.cshtml", survey);
        }

        #endregion


        
    }
