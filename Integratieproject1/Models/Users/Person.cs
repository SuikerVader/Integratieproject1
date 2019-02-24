using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.Models.Users
{
    public class Person : LoggedInUser
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public Sex Sex { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
