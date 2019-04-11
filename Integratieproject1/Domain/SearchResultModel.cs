using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Integratieproject1.Domain
{
    public class SearchResultModel : PageModel
    {
        [BindProperty(SupportsGet = true)] public string SearchString { get; set; }
        public IList<object> SearchResults { get; set; }
    }
}