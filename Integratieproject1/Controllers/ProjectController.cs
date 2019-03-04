using Integratieproject1.BL.Managers;
using Integratieproject1.BL.Models.Ideations;
using Integratieproject1.BL.Models.Projects;
using Microsoft.AspNetCore.Mvc;

namespace Integratieproject1.Controllers
{
    public class ProjectController : Controller
    {
        private ApplicationManager applicationManager;


        public ProjectController()
        {
            applicationManager = new ApplicationManager();
        }

        public IActionResult Project( int projectId)
        {
            Project project = applicationManager.GetProject(projectId);
            return View(project);
        }

        public IActionResult Ideation( int ideationId)
        {
            Ideation ideation = applicationManager.GetIdeation(ideationId);
            return View(ideation);
        }

        public IActionResult Survey(int surveyId)
        {
            BL.Models.Surveys.Survey survey = applicationManager.GetSurvey(surveyId);
            return View(survey);
        }
    }
}