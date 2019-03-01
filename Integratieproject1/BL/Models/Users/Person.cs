using System;

namespace Integratieproject1.BL.Models.Users
{
    public class Person : LoggedInUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Sex Sex { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
