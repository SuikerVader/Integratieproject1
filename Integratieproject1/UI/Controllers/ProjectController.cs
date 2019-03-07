using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Integratieproject1.BL.Managers;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Projects;
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

        [HttpPost]
        public IActionResult SaveFormData(IFormCollection formCollection, int surveyId)
        {
            ArrayList answers = new ArrayList();
            foreach (KeyValuePair<string,StringValues> pair in formCollection)
            {
                answers.Add(pair.Value);
            }
            surveysManager.UpdateAnswers(answers, surveyId);
            Domain.Surveys.Survey survey = surveysManager.GetSurvey(surveyId);
            return View("/UI/Views/Project/Results.cshtml", survey);
        }

        public IActionResult Results(int surveyId)
        {
            Domain.Surveys.Survey survey = surveysManager.GetSurvey(surveyId);
            return View("/UI/Views/Project/Results.cshtml", survey);
        }
    }
}