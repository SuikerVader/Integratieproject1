using System;
using System.Diagnostics;
using Integratieproject1.BL.Managers;
using Integratieproject1.Domain;
using Integratieproject1.Domain.Projects;
using Microsoft.AspNetCore.Mvc;

namespace Integratieproject1.UI.Controllers
{
    public class HomeController : Controller
    {
        private ProjectsManager projectsManager;

        public HomeController()
        {
            projectsManager = new ProjectsManager();
        }
        public IActionResult Index()
        {
            Platform platform = projectsManager.GetPlatform(1);
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

        
    }
}
