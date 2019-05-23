using System.ComponentModel.DataAnnotations;
using Integratieproject1.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Integratieproject1.Domain.Ideations
{
    public class Vote
    {
        [Key] public int VoteId { get; set; }
        public bool Confirmed { get; set; }
        [Required] public VoteType VoteType { get; set; }
        public CustomUser IdentityUser { get; set; }
        [Required] public Idea Idea { get; set; }

    }
}