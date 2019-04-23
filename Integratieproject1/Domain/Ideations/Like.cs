using System.ComponentModel.DataAnnotations;
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
        public IdentityUser IdentityUser { get; set; }
    }
}