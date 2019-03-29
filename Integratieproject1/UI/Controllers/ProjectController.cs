using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Integratieproject1.BL.Managers;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Projects;
using Integratieproject1.Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Integratieproject1.UI.Controllers
{
    public class ProjectController : Controller
    {
        private ProjectsManager projectsManager;
        private IdeationsManager ideationsManager;
        private SurveysManager surveysManager;


        public ProjectController()
        {
            projectsManager = new ProjectsManager();
            ideationsManager = new IdeationsManager();
            surveysManager = new SurveysManager();
            
        }

        public IActionResult Project(int projectId)
        {
            Project project = projectsManager.GetProject(projectId);
            return View("/UI/Views/Project/Project.cshtml", project);
        }

        public IActionResult Ideation(int ideationId)
        {
            Ideation ideation = ideationsManager.GetIdeation(ideationId);
            return View("/UI/Views/Project/Ideation.cshtml", ideation);
        }

        public IActionResult Survey(int surveyId)
        {
            Domain.Surveys.Survey survey = surveysManager.GetSurvey(surveyId);
            return View("/UI/Views/Project/Survey.cshtml", survey);
        }

        public IActionResult Idea(int ideaId)
        {
            Domain.Ideations.Idea idea = ideationsManager.GetIdea(ideaId);
            return View("/UI/Views/Project/Idea.cshtml", idea);
        }
        [HttpPost]    
        public IActionResult PostReaction(IFormCollection formCollection, int ideaId)
        {
            ArrayList parameters = new ArrayList();
            foreach (KeyValuePair<string,StringValues> pair in formCollection)
            {
                parameters.Add(pair.Value);
            }
            ideationsManager.PostReaction(parameters, ideaId);
            Idea idea = ideationsManager.GetIdea(ideaId);
            return View("/UI/Views/Project/Idea.cshtml", idea);
        }

        [HttpPost]
        public IActionResult SaveSurveyFormData(IFormCollection formCollection, int surveyId)
        {
            ArrayList answers = new ArrayList();
            foreach (KeyValuePair<string,StringValues> pair in formCollection)
            {
                answers.Add(pair.Value);
            }
            surveysManager.UpdateAnswers(answers, surveyId);
            Domain.Surveys.Survey survey = surveysManager.GetSurvey(surveyId);
            return View("/UI/Views/Project/SurveyResults.cshtml", survey);
        }

        public IActionResult SurveyResults(int surveyId)
        {
            Domain.Surveys.Survey survey = surveysManager.GetSurvey(surveyId);
            return View("/UI/Views/Project/SurveyResults.cshtml", survey);
        }

        /*public IActionResult CreateVote(int ideaId, VoteType voteType)
        {
            ideationsManager.CreateVote(ideaId,voteType);
            Idea idea = ideationsManager.GetIdea(ideaId);
            return View("/UI/Views/Project/Idea.cshtml", idea);
        }*/
        public IActionResult CreateUserVote(int ideaId, VoteType voteType ,IFormCollection formCollection) 
        {
            ArrayList parameters = new ArrayList();
           
                foreach (KeyValuePair<string,StringValues> pair in formCollection)
                {
                    parameters.Add(pair.Value);
                }

                if (parameters.Count > 0)
                {
                    ideationsManager.CreateVote(ideaId, voteType, parameters[0].ToString());         
                }
                else
                {
                    throw new Exception("fout createVote");
                }
            Idea idea = ideationsManager.GetIdea(ideaId);
            return View("/UI/Views/Project/Idea.cshtml", idea);
        }

        public IActionResult LikeReaction(int ideaId, int reactionId, IFormCollection formCollection)
        {
            ArrayList parameters = new ArrayList();
            foreach (KeyValuePair<string,StringValues> pair in formCollection)
            {
                parameters.Add(pair.Value);
            }
            ideationsManager.LikeReaction(reactionId, parameters[0].ToString());
            Idea idea = ideationsManager.GetIdea(ideaId);
            return View("/UI/Views/Project/Idea.cshtml", idea);

        }

        [HttpPost]
        public void PostIdea(Idea idea, int ideationId, int loggedInUserId)
        {
//            ViewData["ideationId"]

//            ArrayList parameters = new ArrayList();
//            foreach (KeyValuePair<string, StringValues> pair in formCollection)
//            {
//                parameters.Add(pair.Value);
//            }
//            ideationsManager.PostIdea(parameters, ideationId);
//            Ideation ideation = ideationsManager.GetIdeation(ideationId);

            Response.WriteAsync(idea.Title);
            Response.WriteAsync(ideationId.ToString());
            Response.WriteAsync(loggedInUserId.ToString());

//            idea.Ideation = ideation;
//            ideationsManager.CreateIdea(idea);
//            return View("/UI/Views/Project/Ideation.cshtml", ideation);
        }
    }
}