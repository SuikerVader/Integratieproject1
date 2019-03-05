using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Projects;

namespace Integratieproject1.Domain.Users
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
    [Required]
        public string Email { get; set; }
        public ICollection<Vote> Votes { get; set; }
    [Required] 
    public Platform Platform { get; set; }
    /*[ForeignKey("Platform")]
    public int PlatformFK { get; set; }*/
  }
}
