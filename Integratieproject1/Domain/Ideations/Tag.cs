using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Integratieproject1.Domain.Ideations
{
    public class Tag
    {
        [Key]
        public int TagId { get; set; }
        public string TagName { get; set; }
    }
}