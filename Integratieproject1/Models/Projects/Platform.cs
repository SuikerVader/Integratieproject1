using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Integratieproject1.Models.Datatypes;
using Integratieproject1.Models.Users;

namespace Integratieproject1.Models.Projects
{
    public class Platform
    {
      [Key]
        public int PlatformId { get; set; }
    [Required]
        public string PlatformName { get; set; }
        public string Logo { get; set; }
        public Address Adress { get; set; }
        public string Phonenumber { get; set; }
        public string Description { get; set; }
    
        public IList<Project> Projects { get; set; }
        public IList<User> Users { get; set; }
    }
}
