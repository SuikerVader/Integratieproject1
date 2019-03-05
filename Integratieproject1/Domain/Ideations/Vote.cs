using System.ComponentModel.DataAnnotations;
using Integratieproject1.Domain.Users;

namespace Integratieproject1.Domain.Ideations
{
    public class Vote
    {
        [Key]
        public int VoteNr { get; set; }
        public bool Confirmed { get; set; }
    [Required]
        public VoteType VoteType { get; set; }

        public User User { get; set; }
        public Idea Idea { get; set; }
    
    /*[ForeignKey("User")]
    public int UserFK { get; set; }
    [Required]
    [ForeignKey("Idea")]
    public int IdeaFK { get; set; }*/
  }
}
