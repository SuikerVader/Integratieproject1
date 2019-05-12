using System.ComponentModel.DataAnnotations;
using Integratieproject1.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Integratieproject1.Domain.Ideations
{
    public class Like
    {
        [Key]
        public int LikeId { get; set; }
        [Required]
        public Reaction Reaction { get; set; }
        [Required]
        public CustomUser IdentityUser { get; set; }
    }
}