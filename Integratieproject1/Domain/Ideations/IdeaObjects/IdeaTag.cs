using System.ComponentModel.DataAnnotations;
using Integratieproject1.Domain.Ideations;

namespace Integratieproject1.Domain.Datatypes
{
    public class IdeaTag
    {
        [Key]
        public int IdeaTagId { get; set; }
        [Required]
        public Idea Idea { get; set; }
        [Required]
        public Tag Tag { get; set; }
    }
}