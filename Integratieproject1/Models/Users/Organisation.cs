using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.Models.Users
{
    public class Organisation : LoggedInUser
    {
        public String OrganisationName { get; set; }
        public String TaxNumber { get; set; }
    }
}
