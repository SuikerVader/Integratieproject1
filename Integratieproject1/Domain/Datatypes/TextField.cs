using System.ComponentModel.DataAnnotations;
using Integratieproject1.Domain.Ideations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Integratieproject1.Domain.Datatypes
{
    public class TextField
    {
        [Key]
        public int TextId { get; set; }
        public string Text { get; set; }
        [Required] public Idea Idea { get; set; }
        public int OrderNr { get; set; }
    }
}