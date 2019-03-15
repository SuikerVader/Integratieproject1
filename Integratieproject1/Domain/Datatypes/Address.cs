using System.ComponentModel.DataAnnotations;

namespace Integratieproject1.Domain.Datatypes
{
    public class Address
    {
    [Key]
        public int AddressId { get; set; }
        public string Street { get; set; }
        public string HouseNr { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
    }
}