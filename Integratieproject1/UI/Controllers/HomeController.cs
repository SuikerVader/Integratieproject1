using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Integratieproject1.BL.Managers;
using Integratieproject1.Domain;
using Integratieproject1.Domain.Projects;
using Microsoft.AspNetCore.Mvc;

namespace Integratieproject1.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProjectsManager _projectsManager;
        private readonly IdeationsManager _ideationsManager;

        public HomeController()
        {
            _projectsManager = new ProjectsManager();
            _ideationsManager = new IdeationsManager();
        }

        public IActionResult Index()
        {
            Platform platform = _projectsManager.GetPlatform(1);
            return View("/UI/Views/Home/Index.cshtml", platform);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View("/UI/Views/Home/About.cshtml");
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View("/UI/Views/Home/Contact.cshtml");
        }

        public IActionResult Privacy()
        {
            return View("/UI/Views/Home/Privacy.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("/UI/Views/Shared/Error.cshtml",
                new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public IActionResult Search(string searchString)
        {
            const int platformId = 1;

            var projects = _projectsManager.GetProjects(platformId);
            var phases = _projectsManager.GetAllPhases(platformId);
            var ideations = _ideationsManager.GetAllIdeations(platformId);
            var ideas = _ideationsManager.GetAllIdeas(platformId);
            var reactions = _ideationsManager.GetAllReactions(platformId);

            List<object> searchResults = new List<object>();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchResults.AddRange(projects
                    .Where(p => string.IsNullOrEmpty(p.Description)
                        ? p.ProjectName.Contains(searchString)
                        : p.ProjectName.Contains(searchString) || p.Description.Contains(searchString))
                    .ToList()
                );

                searchResults.AddRange(phases
                    .Where(p => string.IsNullOrEmpty(p.Description)
                        ? p.PhaseName.Contains(searchString)
                        : p.PhaseName.Contains(searchString) || p.Description.Contains(searchString))
                    .ToList()
                );

                searchResults.AddRange(ideations
                    .Where(i => i.CentralQuestion.Contains(searchString))
                    .ToList()
                );

                searchResults.AddRange(ideas
                    .Where(i => string.IsNullOrEmpty(i.Text)
                        ? i.Title.Contains(searchString)
                        : i.Title.Contains(searchString) || i.Text.Contains(searchString))
                    .ToList()
                );

                searchResults.AddRange(reactions
                    .Where(r => r.ReactionText.Contains(searchString))
                    .ToList()
                );
            }

            return View("/UI/Views/Shared/SearchResult.cshtml", new SearchResultModel
                {
                    SearchString = searchString,
                    SearchResults = searchResults
                }
            );
        }
    }
}