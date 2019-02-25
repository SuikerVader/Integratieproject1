using Integratieproject1.Models.Datatypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.Models.Ideations
{
    public class Idea
    {
        [Key]
        public int IdeaId { get; set; }
        public Position Position { get; set; }
        public String Video { get; set; }
        public String Image { get; set; }
        public String Theme { get; set; }
        public String Text { get; set; }
        [Required]
        public String Title { get; set; }
    [Required]
    [ForeignKey("LoggedInUser")]
    public int LoggedInUserFk { get; set; }
    [Required]
    [ForeignKey("Ideation")]
    public int IdeationFk { get; set; }
  }
}
