using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Integratieproject1.Models.Ideations;
using Integratieproject1.Models.Projects;

namespace Integratieproject1.Models.Users
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
