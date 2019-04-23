using System.ComponentModel.DataAnnotations;
using Integratieproject1.Domain.Ideations;

namespace Integratieproject1.Domain.Datatypes
{
    public class Image
    {
        [Key] public int ImageId { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
        [Required] public Idea Idea { get; set; }
    }
}