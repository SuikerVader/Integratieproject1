using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.Models.Users
{
    public class Organisation : LoggedInUser
    {
        public string OrganisationName { get; set; }
        public string TaxNumber { get; set; }
    }
}
