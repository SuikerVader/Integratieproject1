using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Claims;
using Integratieproject1.BL.Managers;
using Integratieproject1.Domain.Datatypes;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Projects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Integratieproject1.UI.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectsManager _projectsManager;
        private readonly IdeationsManager _ideationsManager;
        private readonly SurveysManager _surveysManager;
        private readonly DataTypeManager _dataTypeManager;

        public ProjectController()
        {
            _projectsManager = new ProjectsManager();
            _ideationsManager = new IdeationsManager();
            _surveysManager = new SurveysManager();
            _dataTypeManager = new DataTypeManager();   
        }

        public IActionResult Project(int projectId)
        {
            Project project = _projectsManager.GetProject(projectId);
            return View("/UI/Views/Project/Project.cshtml", project);
        }

        public IActionResult Ideation(int ideationId)
        {
            Ideation ideation = _ideationsManager.GetIdeation(ideationId);
            return View("/UI/Views/Project/Ideation.cshtml", ideation);
        }

        public IActionResult Survey(int surveyId)
        {
            Domain.Surveys.Survey survey = _surveysManager.GetSurvey(surveyId);
            return View("/UI/Views/Project/Survey.cshtml", survey);
        }

        public IActionResult Idea(int ideaId)
        {
            Idea idea = _ideationsManager.GetIdea(ideaId);
            return View("/UI/Views/Project/Idea.cshtml", idea);
        }

        [HttpPost]
        public IActionResult PostReaction(IFormCollection formCollection, int ideaId)
        {
            ArrayList parameters = new ArrayList();
            foreach (KeyValuePair<string, StringValues> pair in formCollection)
            {
                parameters.Add(pair.Value);
            }
            
            ClaimsPrincipal currentUser = User;
            string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            _ideationsManager.PostReaction(parameters, ideaId, currentUserId);
            Idea idea = _ideationsManager.GetIdea(ideaId);
            return View("/UI/Views/Project/Idea.cshtml", idea);
        }

        [HttpPost]
        public IActionResult SaveSurveyFormData(IFormCollection formCollection, int surveyId)
        {
            ArrayList answers = new ArrayList();
            foreach (KeyValuePair<string, StringValues> pair in formCollection)
            {
                answers.Add(pair.Value);
            }

            _surveysManager.UpdateAnswers(answers, surveyId);
            Domain.Surveys.Survey survey = _surveysManager.GetSurvey(surveyId);
            return View("/UI/Views/Project/SurveyResults.cshtml", survey);
        }

        public IActionResult SurveyResults(int surveyId)
        {
            Domain.Surveys.Survey survey = _surveysManager.GetSurvey(surveyId);
            return View("/UI/Views/Project/SurveyResults.cshtml", survey);
        }

        public IActionResult CreateVote(int ideaId, VoteType voteType)
        {
            ClaimsPrincipal currentUser = User;
            string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            _ideationsManager.CreateVote(ideaId, voteType, currentUserId);
            Idea idea = _ideationsManager.GetIdea(ideaId);
            return View("/UI/Views/Project/Idea.cshtml", idea);
        }

        [HttpPost]
        public IActionResult CreateUserVote(int ideaId, VoteType voteType, IFormCollection formCollection)
        {
            ArrayList parameters = new ArrayList();

            foreach (KeyValuePair<string, StringValues> pair in formCollection)
            {
                parameters.Add(pair.Value);
            }

            if (parameters.Count > 0)
            {
                _ideationsManager.CreateVote(ideaId, voteType, parameters[0].ToString());
            }
            else
            {
                throw new Exception("fout createVote");
            }

            Idea idea = _ideationsManager.GetIdea(ideaId);
            return View("/UI/Views/Project/Idea.cshtml", idea);
        }

        public IActionResult LikeReaction(int ideaId, int reactionId, IFormCollection formCollection)
        {
            ArrayList parameters = new ArrayList();
            foreach (KeyValuePair<string, StringValues> pair in formCollection)
            {
                parameters.Add(pair.Value);
            }

            ClaimsPrincipal currentUser = User;
            string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            _ideationsManager.LikeReaction(reactionId, parameters[0].ToString(), currentUserId);
            Idea idea = _ideationsManager.GetIdea(ideaId);
            return View("/UI/Views/Project/Idea.cshtml", idea);
        }

        [HttpPost]
        public IActionResult PostIdea(IFormCollection formCollection, List<IFormFile> formFiles, int ideationId)
        {
            ArrayList parameters = new ArrayList();

            foreach (KeyValuePair<string, StringValues> pair in formCollection)
            {
                parameters.Add(pair.Value);
            }

            if (parameters.Count > 0)
            {
                ClaimsPrincipal currentUser = User;
                string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
 
                Idea idea = _ideationsManager.PostIdea(parameters, ideationId, currentUserId);

                if (formFiles.Count > 0)
                {
                    UploadImages(formFiles, idea.IdeaId);
                }
                
                Ideation ideation = _ideationsManager.GetIdeation(ideationId);
                return View("/UI/Views/Project/Ideation.cshtml", ideation);
            }
            else
            {
                throw new Exception("fout createIdea");
            }
        }

        private void UploadImages(List<IFormFile> formFiles, int ideaId)
        {
            string wwwroot = "wwwroot/";
            string uploads = "/images/uploads/";
            string path = wwwroot + uploads;

            foreach (var file in formFiles)
            {
                if (file.Length > 0)
                {
                    string imagePath = Guid.NewGuid() + Path.GetExtension(file.FileName);

                    using (var fileStream = new FileStream(Path.Combine(path, imagePath), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    _dataTypeManager.CreateImage(Path.GetFileName(file.FileName), Path.Combine(uploads, imagePath), ideaId);
                }
            }
        }
    }
}