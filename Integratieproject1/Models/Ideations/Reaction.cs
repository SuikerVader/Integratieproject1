using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Integratieproject1.Models.Ideations
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
