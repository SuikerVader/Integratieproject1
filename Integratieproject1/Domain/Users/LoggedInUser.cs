using System.Collections.Generic;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Projects;

namespace Integratieproject1.Domain.Users
{
    public class LoggedInUser : User
    {
        public string Password { get; set; }
        public bool Verified { get; set; }
        public string ZipCode { get; set; }
        public RoleType RoleType { get; set; }
        
        public ICollection<Reaction> Reactions { get; set; }
        public ICollection<Idea> Ideas { get; set; }
        public ICollection<AdminProject> AdminProjects { get; set; }
    
  }
}
