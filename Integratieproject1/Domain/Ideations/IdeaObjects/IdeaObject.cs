using System.ComponentModel.DataAnnotations;
using Integratieproject1.Domain.Ideations;

namespace Integratieproject1.Domain.Datatypes
{
    public class IdeaObject
    {
        [Key]
        public int IdeaObjectId { get; set; }
        public int OrderNr { get; set; }
        [Required] public Idea Idea { get; set; }
    }
}