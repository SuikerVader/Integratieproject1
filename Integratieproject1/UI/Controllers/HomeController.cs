using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using Integratieproject1.BL.Managers;
using Integratieproject1.Domain;
using Integratieproject1.Domain.Datatypes;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.IoT;
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
        private readonly IoTManager _ioTManager;

        public HomeController()
        {
            _projectsManager = new ProjectsManager();
            _ideationsManager = new IdeationsManager();
            _ioTManager = new IoTManager();
        }

        public IActionResult Index(string platformName)
        {            
            try
            {
                Platform platform = _projectsManager.GetPlatformByName(platformName);
                List<IoTSetup> ioTSetups = _ioTManager.GetAllIoTSetupsForPlatform(platform.PlatformId);
                if (ioTSetups != null && ioTSetups.Count > 0)
                {
                    ViewBag.hasIots = true;
                }
                else
                {
                    ViewBag.hasIots = false;
                }

                return View("/UI/Views/Home/Index.cshtml", platform);
            }
            catch
            {
                return NotFound();
            }
            
        }

        public IActionResult About()
        {   
            return View("/UI/Views/Home/About.cshtml");
        }

        public IActionResult FAQ()
        {
            return View("/UI/Views/Home/FAQ.cshtml");
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
                Console.WriteLine("exceptionFeature not null");
                
                // Get which route the exception occurred at
                string routeWhereExceptionOccurred = exceptionFeature.Path;

                // Get the exception that occurred
                Exception exceptionThatOccurred = exceptionFeature.Error;

                if (exceptionThatOccurred.InnerException != null)
                {
                    exceptionThatOccurred = exceptionThatOccurred.InnerException;
                }
                
                MailService.SendErrorMail( 
                    routeWhereExceptionOccurred, 
                    exceptionThatOccurred, 
                    User.Identity
                );
            }

            return View("/UI/Views/Shared/Error.cshtml",
                new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public IActionResult Search(string platformName, string searchString)
        {
            Platform platform = _projectsManager.GetPlatformByName(platformName);
            int platformId = platform.PlatformId;

            var projects = _projectsManager.GetProjects(platformId);
            var phases = _projectsManager.GetAllPhases(platformId);
            var ideations = _ideationsManager.GetIdeationsByPlatform(platformId);
            var ideas = _ideationsManager.GetAllIdeas(platformId);
            var reactions = _ideationsManager.GetAllReactions(platformId);

            List<object> searchResults = new List<object>();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
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
                    List<string> tagNames = new List<string>();
                    foreach (IdeaTag ideaTag in idea.IdeaTags)
                    {
                        tagNames.Add(ideaTag.Tag.TagName.ToLower());
                    }
                    if (idea.Title.ToLower().Contains(searchString) || tagNames.Contains(searchString))
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


        public IActionResult IoTMap(int id, string type)
        {
            List<IoTSetup> ioTSetups = new List<IoTSetup>();
            if (type.Equals("platform"))
            {
               ioTSetups = _ioTManager.GetAllIoTSetupsForPlatform(id);
                           
            } else if(type.Equals("project"))
            {
                ioTSetups = _ioTManager.GetAllIoTSetupsForProject(id);  
            }
            else if (type.Equals("ideation"))
            {
                ioTSetups = _ioTManager.GetAllIoTSetupsForIdeation(id);
            }
            else if (type.Equals("idea"))
            {
               ioTSetups = _ioTManager.GetAllIoTSetupsForIdea(id);  
            }
            else if (type.Equals("question"))
            {
                ioTSetups = _ioTManager.GetAllIoTSetupsForQuestion(id);
            }

            return View("/UI/Views/Home/IoTMap.cshtml", ioTSetups); 
        }

        public IActionResult UserIdeas(string sortOrder, string searchString)
        {
            ClaimsPrincipal  currentUser = User;
            string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            IEnumerable<Idea> ideas = _ideationsManager.GetIdeasByUserSorted(currentUserId, sortOrder);
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["IdeationSortParm"] = sortOrder == "Ideation" ? "ideation_desc" : "Ideation";
            ViewData["PhaseSortParm"] = sortOrder == "Phase" ? "phase_desc" : "Phase";
            ViewData["ProjectSortParm"] = sortOrder == "Project" ? "project_desc" : "Project";
            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                ideas = ideas.Where(i => i.Title.ToLower().Contains(searchString)
                                               || i.Ideation.CentralQuestion.ToLower().Contains(searchString)
                                               || i.Ideation.Phase.PhaseName.ToLower().Contains(searchString)
                                               || i.Ideation.Phase.Project.ProjectName.ToLower().Contains(searchString));
            }

            return View("/UI/Views/Home/UserIdeas.cshtml", ideas.ToList());
        }
    }
}