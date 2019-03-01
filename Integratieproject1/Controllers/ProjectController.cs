using Integratieproject1.BL.Managers;
using Integratieproject1.BL.Models.Ideations;
using Integratieproject1.BL.Models.Projects;
using Microsoft.AspNetCore.Mvc;

namespace Integratieproject1.UI.Controllers
{
    public class ProjectController : Controller
    {
        private ProjectsManager projectsManager;
        private IdeationsManager ideationsManager;
        private SurveysManager surveysManager;

        public ProjectController()
        {
            this.projectsManager = new ProjectsManager();
            this.ideationsManager = new IdeationsManager();
            this.surveysManager = new SurveysManager();
        }

        public IActionResult Project( int projectId)
        {
            Project project = projectsManager.GetProject(projectId);
            return View(project);
        }

        public IActionResult Ideation( int ideationId)
        {
            Ideation ideation = ideationsManager.GetIdeation(ideationId);
            return View(ideation);
        }

        public IActionResult Survey(int surveyId)
        {
            BL.Models.Surveys.Survey survey = surveysManager.GetSurvey(surveyId);
            return View(survey);
        }
    }
}