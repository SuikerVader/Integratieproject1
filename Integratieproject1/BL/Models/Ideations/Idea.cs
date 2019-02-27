using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Integratieproject1.BL.Models.Datatypes;
using Integratieproject1.BL.Models.IoT;
using Integratieproject1.BL.Models.Users;

namespace Integratieproject1.BL.Models.Ideations
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
    [Required] public LoggedInUser LoggedInUser { get; set; }
    [Required] public Ideation Ideation { get; set; }
    public IList<IoTSetup> IoTSetups { get; set; }
    public IList<Vote> Votes { get; set; }
        
        
        /*[Required]
    [ForeignKey("LoggedInUser")]
    public int LoggedInUserFk { get; set; }
    [Required]
    [ForeignKey("Ideation")]
    public int IdeationFk { get; set; }*/
  }
}
