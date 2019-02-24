using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.Models.Ideations
{
    public class Vote
    {
        [Key]
        public int VoteNr { get; set; }
        public Boolean Confirmed { get; set; }
    [Required]
        public VoteType VoteType { get; set; }
    
    [ForeignKey("User")]
    public int UserFK { get; set; }
    [Required]
    [ForeignKey("Idea")]
    public int IdeaFK { get; set; }
  }
}
