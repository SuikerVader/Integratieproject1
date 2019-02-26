using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Integratieproject1.Models.Users;

namespace Integratieproject1.Models.Ideations
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
