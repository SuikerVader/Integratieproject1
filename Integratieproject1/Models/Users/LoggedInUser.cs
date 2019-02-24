using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.Models.Users
{
    public class LoggedInUser : User
    {
        public String Password { get; set; }
        public Boolean Verified { get; set; }
        public String ZipCode { get; set; }
        public RoleType RoleType { get; set; }

    
  }
}
