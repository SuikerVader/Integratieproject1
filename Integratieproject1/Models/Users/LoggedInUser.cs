using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.Models.Users
{
    public class LoggedInUser
    {
        public String password { get; set; }
        public Boolean verified { get; set; }
        public String zipCode { get; set; }
        public RoleType roleType { get; set; }
    }
}
