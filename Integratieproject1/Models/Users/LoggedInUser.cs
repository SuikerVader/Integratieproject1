using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Integratieproject1.Models.Ideations;

namespace Integratieproject1.Models.Users
{
    public class LoggedInUser : User
    {
        public string Password { get; set; }
        public bool Verified { get; set; }
        public string ZipCode { get; set; }
        public RoleType RoleType { get; set; }
        
        public IList<Reaction> Reactions { get; set; }
        public IList<Idea> Ideas { get; set; }
    
  }
}
