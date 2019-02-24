using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.Models.Projects
{
    public class Platform
    {
      [Key]
        public int PlatformId { get; set; }
    [Required]
        public String PlatformName { get; set; }
        public String Logo { get; set; }
        public Address Adress { get; set; }
        public String Phonenumber { get; set; }
        public String Description { get; set; }

    }
}
