using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Integratieproject1.BL.Managers;
using Integratieproject1.Domain;
using Integratieproject1.Domain.Datatypes;
using Integratieproject1.Domain.Projects;
using Integratieproject1.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
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

        public IActionResult Index(string platformName)
        {
            try{
                Platform platform = _projectsManager.GetPlatformByName(platformName);
                return View("/UI/Views/Home/Index.cshtml", platform);
            }
            catch
            {
                return NotFound();
            }
            
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
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionFeature != null)
            {
                // Get which route the exception occurred at
                string routeWhereExceptionOccurred = exceptionFeature.Path;

                // Get the exception that occurred
                Exception exceptionThatOccurred = exceptionFeature.Error;

                if (exceptionThatOccurred.InnerException != null)
                {
                    exceptionThatOccurred = exceptionThatOccurred.InnerException;
                }
                
                MailService.SendErrorMail(
                    "info.cityofideas@gmail.com", 
                    "CoIMySweet16", 
                    routeWhereExceptionOccurred, 
                    exceptionThatOccurred, 
                    User.Identity);
            }

            return View("/UI/Views/Shared/Error.cshtml",
                new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public IActionResult Search(string searchString)
        {
            const int platformId = 1;

            searchString = searchString.ToLower();

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
                        ? p.ProjectName.ToLower().Contains(searchString)
                        : p.ProjectName.ToLower().Contains(searchString) || p.Description.ToLower().Contains(searchString))
                    .ToList()
                );

                searchResults.AddRange(phases
                    .Where(p => string.IsNullOrEmpty(p.Description)
                        ? p.PhaseName.ToLower().Contains(searchString)
                        : p.PhaseName.ToLower().Contains(searchString) || p.Description.ToLower().Contains(searchString))
                    .ToList()
                );

                searchResults.AddRange(ideations
                    .Where(i => i.CentralQuestion.ToLower().Contains(searchString))
                    .ToList()
                );

                foreach (var idea in ideas)
                {
                    if (idea.Title.ToLower().Contains(searchString))
                        searchResults.Add(idea);
                    
                    if (idea.IdeaObjects != null && idea.GetTextFields() != null)
                    {
                        foreach (var tf in idea.GetTextFields())
                        {
                            if (!string.IsNullOrEmpty(tf.Text) 
                                && tf.Text.ToLower().Contains(searchString))
                            {
                                searchResults.Add(tf);
                            }
                        }
                    }
                }

                searchResults.AddRange(reactions
                    .Where(r => r.ReactionText.ToLower().Contains(searchString))
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

        [HttpGet]
        public IActionResult LogoPartial(string platformName)
        {
           
                Platform platform = _projectsManager.GetPlatformByName(platformName);
                ViewBag.Logo = platform.Logo;
                return PartialView("/UI/Views/Shared/_LogoPartial.cshtml");
            
        }
    }
}