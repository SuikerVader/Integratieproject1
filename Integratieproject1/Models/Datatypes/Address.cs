using System;
using System.ComponentModel.DataAnnotations;

namespace Integratieproject1.Models.Projects
{
    public class Address
    {
    [Key]
        public int AddressId { get; set; }
        public String Street { get; set; }
        public String HouseNr { get; set; }
        public String City { get; set; }
        public String ZipCode { get; set; }
    }
}