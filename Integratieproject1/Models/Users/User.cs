using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.Models.Users
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
    [Required]
        public String Email { get; set; }
    [Required]
    [ForeignKey("Platform")]
    public int PlatformFK { get; set; }
  }
}
