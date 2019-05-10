using System.ComponentModel.DataAnnotations;
using Integratieproject1.Domain.Users;
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
        public CustomUser Admin { get; set; }
    }
}