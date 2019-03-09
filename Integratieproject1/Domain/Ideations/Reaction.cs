using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Integratieproject1.Domain.Users;

namespace Integratieproject1.Domain.Ideations
{
    public class Reaction
    {
        [Key]
        public int ReactionId { get; set; }
        public string ReactionText { get; set; }
    [Required] 
    public LoggedInUser LoggedInUser { get; set; }
    public Ideation Ideation { get; set; }
    public Idea Idea { get; set; }
    
    public ICollection<Like> Likes { get; set; }
    
    
    /*[ForeignKey("LoggedInUser")]
    public int LoggedInUserFK { get; set; }
    [ForeignKey("Ideation")]
    public int IdeationFK { get; set; }
    [ForeignKey("Idea")]
    public int IdeaFK { get; set; }*/
    public Reaction()
    {
        
    }
    public Reaction(Idea idea)
    {
        Idea = idea;
    }
    }
}
