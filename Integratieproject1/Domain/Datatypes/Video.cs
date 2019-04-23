

using System.ComponentModel.DataAnnotations;
using Integratieproject1.Domain.Ideations;

namespace Integratieproject1.Domain.Datatypes
{
    public class Video
    {
        [Key]
        public int VideoId { get; set; }
        public string Url { get; set; }
        [Required]
        public Idea Idea { get; set; }
        public int OrderNr { get; set; }
    }
}