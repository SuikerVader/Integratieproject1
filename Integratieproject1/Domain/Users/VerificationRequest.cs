using System.ComponentModel.DataAnnotations;


namespace Integratieproject1.Domain.Users
{
    public class VerificationRequest
    {
        [Key] public User user { get; set; }
        public string request { get; set; }
    }
}