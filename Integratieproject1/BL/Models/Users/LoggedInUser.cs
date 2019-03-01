using System.Collections.Generic;
using Integratieproject1.BL.Models.Ideations;

namespace Integratieproject1.BL.Models.Users
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
