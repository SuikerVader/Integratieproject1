using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.Domain.Users
{
    public class CustomUser : IdentityUser
    {
        public string Surname { get; set; } = "";
        public string Name { get; set; } = "";
        public string Sex { get; set; } = "";
        public int Age { get; set; } = 0;
        public string Zipcode { get; set; }
        public bool Verified { get; set; } = false;
        public bool AskVerify { get; set; } = false;
    }
}
