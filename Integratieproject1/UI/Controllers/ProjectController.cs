using Integratieproject1.BL.Managers;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Projects;
using Microsoft.AspNetCore.Mvc;

namespace Integratieproject1.UI.Controllers
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
            return View("/UI/Views/Project/Project.cshtml", project);
        }

        public IActionResult Ideation( int ideationId)
        {
            Ideation ideation = applicationManager.GetIdeation(ideationId);
            return View("/UI/Views/Project/Ideation.cshtml", ideation);
        }

        public IActionResult Survey(int surveyId)
        {
            Domain.Surveys.Survey survey = applicationManager.GetSurvey(surveyId);
            return View("/UI/Views/Project/Survey.cshtml", survey);
        }
    }
}