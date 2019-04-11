using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
            return View("/UI/Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Search(string searchString)
        {
            const int platformId = 1;
            
            var projects = _projectsManager.GetProjects(platformId);
            var phases = _projectsManager.GetAllPhases(platformId);
            var ideations = _ideationsManager.GetAllIdeations(platformId);
            var ideas = _ideationsManager.GetAllIdeas(platformId);
            var reactions = _ideationsManager.GetAllReactions(platformId);
            
            if (!string.IsNullOrEmpty(searchString))
            {
                projects = projects
                    .Where(p => string.IsNullOrEmpty(p.Description) ? 
                        p.ProjectName.Contains(searchString) : 
                        p.ProjectName.Contains(searchString) || p.Description.Contains(searchString))
                    .ToList();
                
                phases = phases
                    .Where(p => string.IsNullOrEmpty(p.Description) ? 
                        p.PhaseName.Contains(searchString) : 
                        p.PhaseName.Contains(searchString) || p.Description.Contains(searchString))
                    .ToList();
                
                ideations = ideations
                    .Where(i => i.CentralQuestion.Contains(searchString))
                    .ToList();
                
                ideas = ideas
                    .Where(i => string.IsNullOrEmpty(i.Text) ? 
                        i.Title.Contains(searchString) : 
                        i.Title.Contains(searchString) || i.Text.Contains(searchString))
                    .ToList();
                
                reactions = reactions
                    .Where(r => r.ReactionText.Contains(searchString))
                    .ToList();
            }
            
            ArrayList searchedResults = new ArrayList();
            searchedResults.Add(projects);
            searchedResults.Add(phases);
            searchedResults.Add(ideations);
            searchedResults.Add(ideas);
            searchedResults.Add(reactions);
            
            return View("/UI/Views/Shared/SearchResult.cshtml", new SearchResultModel
                {
                    SearchString = searchString, 
                    SearchedProjects = projects, 
                    SearchedPhases = phases, 
                    SearchedIdeations = ideations, 
                    SearchedIdeas = ideas, 
                    SearchedReactions = reactions
                }
            );
        }
    }
}
