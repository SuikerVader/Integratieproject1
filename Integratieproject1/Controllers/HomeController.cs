using System.Diagnostics;
using Integratieproject1.BL.Managers;
using Integratieproject1.BL.Models;
using Integratieproject1.BL.Models.Projects;
using Microsoft.AspNetCore.Mvc;

namespace Integratieproject1.Controllers
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
            return View(platform);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}
