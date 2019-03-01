using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Integratieproject1.BL.Models.Ideations;
using Integratieproject1.BL.Models.Projects;

namespace Integratieproject1.BL.Models.Users
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
    [Required]
        public string Email { get; set; }
        public IList<Vote> Votes { get; set; }
    [Required] 
    public Platform Platform { get; set; }
    /*[ForeignKey("Platform")]
    public int PlatformFK { get; set; }*/
  }
}
