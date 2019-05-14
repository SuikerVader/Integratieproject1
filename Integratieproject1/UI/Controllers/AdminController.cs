using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Integratieproject1.BL.Managers;
using Integratieproject1.DAL;
using Integratieproject1.Domain;
using Integratieproject1.Domain.Datatypes;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.IoT;
using Integratieproject1.Domain.Projects;
using Integratieproject1.Domain.Surveys;
using Integratieproject1.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Integratieproject1.UI.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class AdminController : Controller
    {
        private readonly ProjectsManager _projectsManager;
        private readonly IdeationsManager _ideationsManager;
        private readonly SurveysManager _surveysManager;
        private readonly UsersManager _usersManager;
        private readonly IoTManager _ioTManager;

        public AdminController()
        {
            _projectsManager = new ProjectsManager();
            _usersManager = new UsersManager();
            _ideationsManager = new IdeationsManager();
            _surveysManager = new SurveysManager();
            _ioTManager = new IoTManager();
            
        }

        public IActionResult Admin()
        {
            ClaimsPrincipal currentUser = User;
            string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            CustomUser user = _usersManager.GetUser(currentUserId);
            return View("/UI/Views/Admin/Admin.cshtml", user);
        }

        public IActionResult Moderators()
        {
            IList<CustomUser> mods = _usersManager.GetUsers("MOD");
            return View("/UI/Views/Admin/Moderators.cshtml", mods);
        }

        public IActionResult DeleteModRole(string modId)
        {
            _usersManager.DeleteRole(modId, "MOD");
            _usersManager.GiveRole(modId, "USER");
            IList<CustomUser> mods = _usersManager.GetUsers("MOD");
            return View("/UI/Views/Admin/Moderators.cshtml", mods);
        }

        public IActionResult GiveModRole(string userId)
        {
            _usersManager.GiveRole(userId, "MOD");
            IList<CustomUser> users = _usersManager.GetUsers("USER");
            return View("/UI/Views/Admin/Users.cshtml", users);
        }

        public IActionResult DeleteMod(string modId)
        {
            _usersManager.DeleteUser(modId);
            IList<CustomUser> mods = _usersManager.GetUsers("MOD");
            return View("/UI/Views/Admin/Moderators.cshtml", mods);
        }

        public IActionResult Users()
        {
            IList<CustomUser> users = _usersManager.GetUsers("USER");
            return View("/UI/Views/Admin/Users.cshtml", users);
        }
        public IActionResult AddIoT(int id, string type)
        {
            if (type.Equals("question"))
            {
                IoTSetup ioTSetup = new IoTSetup()
                {
                    Question = _surveysManager.GetQuestion(id),
                    Position = new Position(){Lat = "0", Lng = "0"}
                };
                return View("/UI/Views/Admin/AddIoTSetupToQuestion.cshtml", ioTSetup);
            }
            else
            {
                IoTSetup ioTSetup = new IoTSetup()
                {
                    Idea = _ideationsManager.GetIdea(id),
                    Position = new Position(){Lat = "0", Lng = "0"}
                };
                return View("/UI/Views/Admin/AddIoTSetupToIdea.cshtml", ioTSetup);
            }  
        }
        [HttpPost]
        public IActionResult AddIoT(IoTSetup ioTSetup,string type , int id )
        {
            _ioTManager.CreateIoTSetup(ioTSetup, id, type);
            if (type.Equals("question"))
            {
                Survey survey = _surveysManager.GetSurvey(_surveysManager.GetQuestion(id).Survey.SurveyId);
                return View("/UI/Views/Admin/EditSurvey.cshtml", survey);
            }
            else
            {
                Idea idea = _ideationsManager.GetIdea(id);
                ViewBag.tags = _ideationsManager.GetTags(idea.IdeaId);
                return View("/UI/Views/Project/EditIdea.cshtml", idea);
            }
        }
        public IActionResult EditIoT(string iotId)
        {
            IoTSetup ioTSetup = _ioTManager.GetIoT(iotId);
            return View("/UI/Views/Admin/EditIoTSetup.cshtml", ioTSetup);
        }
        [HttpPost]
        public IActionResult EditIoT(string iotId,IoTSetup ioTSetup)
        {
            _ioTManager.EditIoTSetup(ioTSetup, iotId);
            IoTSetup returnIoTSetup = _ioTManager.GetIoT(iotId);
            if (returnIoTSetup.Question != null)
            {
                Survey survey = _surveysManager.GetSurvey(_surveysManager.GetQuestion(returnIoTSetup.Question.QuestionId).Survey.SurveyId);
                return View("/UI/Views/Admin/EditSurvey.cshtml", survey);
            }
            else
            {
                Idea idea = _ideationsManager.GetIdea(returnIoTSetup.Idea.IdeaId);
                ViewBag.tags = _ideationsManager.GetTags(idea.IdeaId);
                return View("/UI/Views/Project/EditIdea.cshtml", idea);
            }
        }
        public IActionResult DeleteIoT(string iotId)
        {
            bool returnQuestion =  _ioTManager.GetIoT(iotId).Question != null;
            int returnId = 0;
            if (returnQuestion)
            {
               returnId = _ioTManager.GetIoT(iotId).Question.QuestionId;
            }
            else
            {
                returnId = _ioTManager.GetIoT(iotId).Idea.IdeaId;
            }
            _ioTManager.DeleteIoTSetup(iotId);
            
            if (returnQuestion)
            {
                Survey survey = _surveysManager.GetSurvey(_surveysManager.GetQuestion(returnId).Survey.SurveyId);
                return View("/UI/Views/Admin/EditSurvey.cshtml", survey);
            }
            else
            {
                Idea idea = _ideationsManager.GetIdea(returnId);
                ViewBag.tags = _ideationsManager.GetTags(idea.IdeaId);
                return View("/UI/Views/Project/EditIdea.cshtml", idea);
            }
            
            
        }

        #region VerificationRequests

        public IActionResult VerificationRequests()
        {
            IList<VerificationRequest> requests = _usersManager.GetVerificationRequests().ToList();
            return View("/UI/Views/Admin/VerificationRequests.cshtml", requests);
        }

        public IActionResult HandleVerificationRequest(VerificationRequest request, bool acc)
        {
            _usersManager.HandleVerificationRequest(request, acc);
            IList<VerificationRequest> requests = _usersManager.GetVerificationRequests().ToList();
            return View("/UI/Views/Admin/VerificationRequests.cshtml", requests);
        }

        #endregion

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
        public IActionResult EditProject(int projectId, Project project, IFormFile formFile)
        {
            if (ModelState.IsValid)
            {
                if (formFile != null)
                {
                    project.BackgroundImage = GetImagePath(formFile);
                }
                Project newProject = _projectsManager.EditProject(project, projectId);
                ClaimsPrincipal currentUser = User;
                string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                IList<Project> projects = _projectsManager.GetAdminProjects(currentUserId);
                return View("/UI/Views/Admin/Projects.cshtml", projects);
            }

            Project returnProject = _projectsManager.GetProject(projectId);
            return View("/UI/Views/Admin/EditProject.cshtml", returnProject);
        }
        public IActionResult DeleteBackgroundImage(int projectId)
        {
            _projectsManager.DeleteBackgroundImageProject(projectId);
            Project returnProject = _projectsManager.GetProject(projectId);
            return View("/UI/Views/Admin/EditProject.cshtml", returnProject);
        }

        public IActionResult CreateProject()
        {
            return View("/UI/Views/Admin/CreateProject.cshtml");
        }

        [HttpPost]
        public IActionResult CreateProject(Project project, IFormFile formFile, string platformName)
        {
            if (ModelState.IsValid)
            {
                if (formFile != null)
                {
                    project.BackgroundImage = GetImagePath(formFile);
                }
                try
                {
                    ClaimsPrincipal currentUser = User;
                    string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                    Platform platform = _projectsManager.GetPlatformByName(platformName);
                    _projectsManager.CreateProject(project, currentUserId, platform.PlatformId);
                    IList<Project> projects = _projectsManager.GetAdminProjects(currentUserId);
                    return View("/UI/Views/Admin/Projects.cshtml", projects);
                }
                catch
                {
                    return NotFound();
                }

            }

            return View("/UI/Views/Admin/CreateProject.cshtml");
        }

        public IActionResult DeleteProject(int projectId)
        {
            _projectsManager.DeleteProject(projectId);
            ClaimsPrincipal currentUser = User;
            string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            IList<Project> projects = _projectsManager.GetAdminProjects(currentUserId);
            return View("/UI/Views/Admin/Projects.cshtml", projects);
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

        #endregion

        #region Phase

        public IActionResult Phases(int projectId)
        {
            IList<Phase> phases = _projectsManager.GetPhases(projectId);
            ViewBag.Project = _projectsManager.GetProject(projectId);
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
                ViewBag.Project = _projectsManager.GetProject(editedPhase.Project.ProjectId);
                IList<Phase> phases = _projectsManager.GetPhases(editedPhase.Project.ProjectId);
                return View("/UI/Views/Admin/Phases.cshtml", phases);
            }

            Phase returnPhase = _projectsManager.GetPhase(phaseId);
            return View("/UI/Views/Admin/EditPhase.cshtml", returnPhase);
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
                ViewBag.Project = _projectsManager.GetProject(createdPhase.Project.ProjectId);
                IList<Phase> phases = _projectsManager.GetPhases(createdPhase.Project.ProjectId);
                return View("/UI/Views/Admin/Phases.cshtml", phases);
            }

            Phase returnPhase = _projectsManager.GetNewPhase(projectId);
            return View("/UI/Views/Admin/CreatePhase.cshtml", returnPhase);
        }

        public IActionResult DeletePhase(int phaseId)
        {
            Phase phase = _projectsManager.GetPhase(phaseId);
            int projectId = phase.Project.ProjectId;
            _projectsManager.DeletePhase(phaseId);
            ViewBag.Project = _projectsManager.GetProject(projectId);
            IList<Phase> phases = _projectsManager.GetPhases(projectId);
            return View("/UI/Views/Admin/Phases.cshtml", phases);
        }

        #endregion

        #region Ideation

        public IActionResult Ideations(int phaseId)
        {
            IList<Ideation> ideations = _ideationsManager.GetIdeations(phaseId);
            ViewBag.Phase = _projectsManager.GetPhase(phaseId);
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
                IList<Ideation> ideations = _ideationsManager.GetIdeations(editIdeation.Phase.PhaseId);
                ViewBag.Phase = _projectsManager.GetPhase(editIdeation.Phase.PhaseId);
                return View("/UI/Views/Admin/Ideations.cshtml", ideations);
            }

            Ideation returnIdeation = _ideationsManager.GetIdeation(ideationId);
            return View("/UI/Views/Admin/EditIdeation.cshtml", ideation);
        }

        public IActionResult AddIdeation(int phaseId)
        {
            ViewBag.Phase = _projectsManager.GetPhase(phaseId);
            return View("/UI/Views/Admin/CreateIdeation.cshtml");
        }

        [HttpPost]
        public IActionResult AddIdeation(Ideation ideation, int phaseId)
        {
            if (ModelState.IsValid)
            {
                _ideationsManager.CreateIdeation(ideation, phaseId);
                IList<Ideation> ideations = _ideationsManager.GetIdeations(phaseId);
                ViewBag.Phase = _projectsManager.GetPhase(phaseId);
                return View("/UI/Views/Admin/Ideations.cshtml", ideations);
            }

            ViewBag.Phase = _projectsManager.GetPhase(phaseId);
            return View("/UI/Views/Admin/CreateIdeation.cshtml");
        }

        public IActionResult DeleteIdeation(int ideationId)
        {
            Ideation ideation = _ideationsManager.GetIdeation(ideationId);
            int phaseId = ideation.Phase.PhaseId;
            _ideationsManager.DeleteIdeation(ideationId);
            IList<Ideation> ideations = _ideationsManager.GetIdeations(phaseId);
            ViewBag.Phase = _projectsManager.GetPhase(phaseId);
            return View("/UI/Views/Admin/Ideations.cshtml", ideations);
        }

        public IActionResult Ideas(int ideationId)
        {
            IList<Idea> ideas = _ideationsManager.GetIdeas(ideationId);
            ViewBag.Ideation = _ideationsManager.GetIdeation(ideationId);
            ViewBag.IdeationId = ideationId;
            return View("/UI/Views/Admin/Ideas.cshtml", ideas);
        }

        public IActionResult IdeationResults()
        {
            IList<Ideation> ideations = _ideationsManager.GetAllIdeations(1);
            return View("/UI/Views/Admin/IdeationResults.cshtml", ideations);
        }

        public IActionResult IdeationResult(int ideationId)
        {
            Ideation ideation = _ideationsManager.GetIdeation(ideationId);
            return View("/UI/Views/Admin/IdeationResult.cshtml", ideation);
        }

        #endregion

        #region Survey

        public IActionResult Surveys(int phaseId)
        {
            IList<Survey> surveys = _surveysManager.GetSurveys(phaseId);
            ViewBag.Phase = _projectsManager.GetPhase(phaseId);
            return View("/UI/Views/Admin/Surveys.cshtml", surveys);
        }

        public IActionResult AddSurvey(int phaseId)
        {
            _surveysManager.CreateNewSurvey(phaseId);
            IList<Survey> surveys = _surveysManager.GetSurveys(phaseId);
            ViewBag.Phase = _projectsManager.GetPhase(phaseId);
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
                Survey editsurvey = _surveysManager.EditSurvey(survey, surveyId);
                IList<Survey> surveys = _surveysManager.GetSurveys(editsurvey.Phase.PhaseId);
                ViewBag.Phase = _projectsManager.GetPhase(editsurvey.Phase.PhaseId);
                return View("/UI/Views/Admin/Surveys.cshtml", surveys);
            }

            Survey returnSurvey = _surveysManager.GetSurvey(surveyId);
            IList<Survey> returnSurveys = _surveysManager.GetSurveys(returnSurvey.Phase.PhaseId);
            ViewBag.Phase = _projectsManager.GetPhase(returnSurvey.Phase.PhaseId);
            return View("/UI/Views/Admin/Surveys.cshtml", returnSurveys);
        }

        public IActionResult DeleteSurvey(int surveyId)
        {
            Survey returnSurvey = _surveysManager.GetSurvey(surveyId);
            IList<Survey> returnSurveys = _surveysManager.GetSurveys(returnSurvey.Phase.PhaseId);
            ViewBag.Phase = _projectsManager.GetPhase(returnSurvey.Phase.PhaseId);
            _surveysManager.DeleteSurvey(surveyId);
            return View("/UI/Views/Admin/Surveys.cshtml", returnSurveys);
        }

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

        public IActionResult QuestionNrUp(int questionId, int surveyId)
        {
            _surveysManager.QuestionNrChange(questionId, "up", surveyId);
            Survey survey = _surveysManager.GetSurvey(surveyId);
            return View("/UI/Views/Admin/EditSurvey.cshtml", survey);
        }

        public IActionResult QuestionNrDown(int questionId, int surveyId)
        {
            _surveysManager.QuestionNrChange(questionId, "down", surveyId);
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

        public IActionResult SurveyResults()
        {
            IList<Survey> surveys = _surveysManager.GetAllSurveys();
            return View("/UI/Views/Admin/SurveyResults.cshtml", surveys);
        }

        #endregion

        #region Tags
        
        public IActionResult Tags()
        {
           List<Tag> tags = _ideationsManager.GetAllTags();
           return View("/UI/Views/Admin/Tags.cshtml", tags);
        }
        public IActionResult EditTag(int tagId)
        {
            Tag tag = _ideationsManager.GetTag(tagId);
            return View("/UI/Views/Admin/EditTag.cshtml", tag);
        }
        
        [HttpPost]
        public IActionResult EditTag(Tag tag, int tagId)
        {
            _ideationsManager.EditTag(tag, tagId);
            List<Tag> tags = _ideationsManager.GetAllTags();
            return View("/UI/Views/Admin/Tags.cshtml", tags);
        }

        public IActionResult DeleteTag(int tagId)
        {
            _ideationsManager.DeleteTag(tagId);
            List<Tag> tags = _ideationsManager.GetAllTags();
            return View("/UI/Views/Admin/Tags.cshtml", tags);
        }

        public IActionResult AddTag()
        {
            return View("/UI/Views/Admin/AddTag.cshtml");
        }
        [HttpPost]
        public IActionResult AddTag(Tag tag)
        {
            _ideationsManager.AddTag(tag);
            List<Tag> tags = _ideationsManager.GetAllTags();
            return View("/UI/Views/Admin/Tags.cshtml", tags);
        }


        #endregion


        
    }
 }