using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;


namespace Integratieproject1.Domain.Users
{
    public class VerificationRequest
    {
        [Key] public int verificationRequestId { get; set; }
        [Required] public IdentityUser user { get; set; }
        [Required] public string request { get; set; }
        public bool? handled { get; set; }
    }
}