using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Integratieproject1.Domain.Projects
{
    public class AdminProject
    {
        [Key]
        public int AdminProjectId { get; set; }
        [Required]
        public Project Project { get; set; }
        [Required]
        public IdentityUser Admin { get; set; }
    }
}