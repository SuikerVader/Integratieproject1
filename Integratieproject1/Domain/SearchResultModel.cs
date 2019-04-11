using System.Collections;
using System.Collections.Generic;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Projects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Integratieproject1.Domain
{
    public class SearchResultModel : PageModel
    {
        [BindProperty(SupportsGet = true)] public string SearchString { get; set; }
        public IList<Project> SearchedProjects { get; set; }
        public IList<Phase> SearchedPhases { get; set; }
        public IList<Ideation> SearchedIdeations { get; set; }
        public IList<Idea> SearchedIdeas { get; set; }
        public IList<Reaction> SearchedReactions { get; set; }
    }
}