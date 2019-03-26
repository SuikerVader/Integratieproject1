using System.ComponentModel.DataAnnotations;
using Integratieproject1.Domain.Users;

namespace Integratieproject1.Domain.Projects
{
    public class AdminProject
    {
        [Key]
        public int AdminProjectId { get; set; }
        [Required]
        public Project Project { get; set; }
        [Required]
        public LoggedInUser Admin { get; set; }
    }
}