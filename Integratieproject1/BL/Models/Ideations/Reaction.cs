using System.ComponentModel.DataAnnotations;
using Integratieproject1.BL.Models.Users;

namespace Integratieproject1.BL.Models.Ideations
{
    public class Reaction
    {
        [Key]
        public int ReactionId { get; set; }
        public string ReactionText { get; set; }
        public int TotalLikes { get; set; }
    [Required] 
    public LoggedInUser LoggedInUser { get; set; }
    public Ideation Ideation { get; set; }
    public Idea Idea { get; set; }
    
    
    /*[ForeignKey("LoggedInUser")]
    public int LoggedInUserFK { get; set; }
    [ForeignKey("Ideation")]
    public int IdeationFK { get; set; }
    [ForeignKey("Idea")]
    public int IdeaFK { get; set; }*/
  }
}
