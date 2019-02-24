using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.Models
{
    public class Reaction
    {
        [Key]
        public int ReactionId { get; set; }
        public String ReactionText { get; set; }
        public int TotalLikes { get; set; }
    [Required]
    [ForeignKey("LoggedInUser")]
    public int LoggedInUserFK { get; set; }
    [ForeignKey("Ideation")]
    public int IdeationFK { get; set; }
    [ForeignKey("Idea")]
    public int IdeaFK { get; set; }
  }
}
