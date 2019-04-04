using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Integratieproject1.BL.Managers;
using Integratieproject1.Domain;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Projects;
using Integratieproject1.Domain.Surveys;
using Integratieproject1.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Integratieproject1.UI.Controllers{}

    [Authorize(Roles="Admin")]
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

        public IActionResult Admin(IdentityUser user)
        {
            return View("/UI/Views/Admin/Admin.cshtml", user);
        }

        #region Project

        public IActionResult Projects(int userId)
        {
            IList<Project> projects = projectsManager.GetAdminProjects(userId);
            return View("/UI/Views/Admin/Projects.cshtml", projects);
        }

        public IActionResult EditProject(int projectId)
        {
            Project project = projectsManager.GetProject(projectId);
            return View("/UI/Views/Admin/EditProject.cshtml", project);
        }

        [HttpPost]
        public IActionResult EditProject(int projectId, Project project)
        {
            if (ModelState.IsValid)
            {
                projectsManager.EditProject(project, projectId);
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
        public IActionResult CreateProject(Project project, int userId)
        {
            if (ModelState.IsValid)
            {
                projectsManager.CreateProject(project, userId);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult DeleteProject(int projectId)
        {
            projectsManager.DeleteProject(projectId);
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Phase

        public IActionResult Phases(int projectId)
        {
            IList<Phase> phases = projectsManager.GetPhases(projectId);
            ViewData["ProjectId"] = projectId;
            return View("/UI/Views/Admin/Phases.cshtml", phases);
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

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AddPhase(int projectId)
        {
            Phase phase = projectsManager.GetNewPhase(projectId);

            return View("/UI/Views/Admin/CreatePhase.cshtml", phase);
        }

        [HttpPost]
        public IActionResult AddPhase(Phase phase, int phaseNr, int projectId)
        {
            if (ModelState.IsValid)
            {
                Phase createdPhase = projectsManager.CreatePhase(phase, phaseNr, projectId);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult DeletePhase(int phaseId)
        {
            projectsManager.DeletePhase(phaseId);
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Ideation

        public IActionResult Ideations(int phaseId)
        {
            IList<Ideation> ideations = ideationsManager.GetIdeations(phaseId);
            ViewData["PhaseId"] = phaseId;
            return View("/UI/Views/Admin/Ideations.cshtml", ideations);
        }

        public IActionResult EditIdeation(int ideationId)
        {
            Ideation ideation = ideationsManager.GetIdeation(ideationId);
            return View("/UI/Views/Admin/EditIdeation.cshtml", ideation);
        }

        [HttpPost]
        public IActionResult EditIdeation(Ideation ideation, int ideationId)
        {
            if (ModelState.IsValid)
            {
                Ideation editIdeation = ideationsManager.EditIdeation(ideation, ideationId);
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
                ideationsManager.CreateIdeation(ideation, phaseId);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult DeleteIdeation(int ideationId)
        {
            ideationsManager.DeleteIdeation(ideationId);
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
            IList<Survey> surveys = surveysManager.GetSurveys(phaseId);
            ViewData["PhaseId"] = phaseId;
            return View("/UI/Views/Admin/Surveys.cshtml", surveys);
        }

        public IActionResult AddSurvey(int phaseId)
        {
            surveysManager.CreateNewSurvey(phaseId);
            IList<Survey> surveys = surveysManager.GetSurveys(phaseId);
            return View("/UI/Views/Admin/Surveys.cshtml", surveys);
        }

        public IActionResult EditSurvey(int surveyId)
        {
            Survey survey = surveysManager.GetSurvey(surveyId);
            return View("/UI/Views/Admin/EditSurvey.cshtml", survey);
        }


        [HttpPost]
        public IActionResult EditSurvey(Survey survey, int surveyId)
        {
            if (ModelState.IsValid)
            {
                surveysManager.EditSurvey(survey, surveyId);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult DeleteSurvey(int surveyId)
        {
            surveysManager.DeleteSurvey(surveyId);
            return RedirectToAction("Index", "Home");
        }

        /*public IActionResult AddQuestion()
        {
            return PartialView("/UI/Views/Admin/_CreateQuestionPartial.cshtml");
        }*/

        [HttpPost]
        public IActionResult AddQuestion(Question question, int surveyId)
        {
            surveysManager.CreateQuestion(question, surveyId);
            Survey survey = surveysManager.GetSurvey(surveyId);
            return View("/UI/Views/Admin/EditSurvey.cshtml", survey);
        }
        [HttpPost]
        public IActionResult EditQuestion(Question question, int questionId, int surveyId)
        {
            surveysManager.EditQuestion(question, questionId, surveyId);
            Survey survey = surveysManager.GetSurvey(surveyId);
            return View("/UI/Views/Admin/EditSurvey.cshtml", survey);
        }
        
        public IActionResult DeleteQuestion(int questionId, int surveyId)
        {
           surveysManager.DeleteQuestion(questionId); 
           Survey survey = surveysManager.GetSurvey(surveyId);
           return View("/UI/Views/Admin/EditSurvey.cshtml", survey);
        }
        
        [HttpPost]
        public IActionResult EditAnswer(Answer answer, int answerId, int questionId, int surveyId)
        {
            surveysManager.EditAnswer(answer, answerId, questionId);
            Survey survey = surveysManager.GetSurvey(surveyId);
            return View("/UI/Views/Admin/EditSurvey.cshtml", survey);
            
        }
        [HttpPost]
        public IActionResult AddAnswer(Answer answer, int questionId, int surveyId)
        {
            answer.Question = surveysManager.GetQuestion(questionId);
            surveysManager.CreateAnswer(answer);
            Survey survey = surveysManager.GetSurvey(surveyId);
            return View("/UI/Views/Admin/EditSurvey.cshtml", survey);
        }
        
        public IActionResult DeleteAnswer(int answerId, int surveyId)
        {
            surveysManager.DeleteAnswer(answerId);
            Survey survey = surveysManager.GetSurvey(surveyId);
            return View("/UI/Views/Admin/EditSurvey.cshtml", survey);
        }

        #endregion


        
    }
