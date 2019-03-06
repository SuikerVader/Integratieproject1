using Integratieproject1.BL.Managers;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Projects;
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
            projectsManager = new ProjectsManager();
            ideationsManager = new IdeationsManager();
            surveysManager = new SurveysManager();
            
        }

        public IActionResult Project( int projectId)
        {
            Project project = projectsManager.GetProject(projectId);
            return View("/UI/Views/Project/Project.cshtml", project);
        }

        public IActionResult Ideation( int ideationId)
        {
            Ideation ideation = ideationsManager.GetIdeation(ideationId);
            return View("/UI/Views/Project/Ideation.cshtml", ideation);
        }

        public IActionResult Survey(int surveyId)
        {
            Domain.Surveys.Survey survey = surveysManager.GetSurvey(surveyId);
            return View("/UI/Views/Project/Survey.cshtml", survey);
        }
    }
}