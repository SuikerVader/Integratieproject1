using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Integratieproject1.Domain.Datatypes;
using Integratieproject1.Domain.IoT;
using Microsoft.AspNetCore.Identity;

namespace Integratieproject1.Domain.Ideations
{
    public class Idea
    {
        [Key] public int IdeaId { get; set; }
        public Position Position { get; set; }
        public String Video { get; set; }
        public String Image { get; set; }
        public String Theme { get; set; }
        public String Text { get; set; }
        [Required] public String Title { get; set; }
        [Required] public IdentityUser IdentityUser { get; set; }
        [Required] public Ideation Ideation { get; set; }
        public ICollection<IoTSetup> IoTSetups { get; set; }
        public ICollection<Vote> Votes { get; set; }
        public ICollection<Reaction> Reactions { get; set; }


        /*[Required]
    [ForeignKey("LoggedInUser")]
    public int LoggedInUserFk { get; set; }
    [Required]
    [ForeignKey("Ideation")]
    public int IdeationFk { get; set; }*/
    }
}