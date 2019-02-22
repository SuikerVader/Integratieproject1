using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.Models.Users
{
    public class Person
    {
        public String firstName { get; set; }
        public String lastName { get; set; }
        public Sex sex { get; set; }
        public DateTime birthDate { get; set; }
    }
}
