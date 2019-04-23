using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Integratieproject1.Domain.Ideations
{
    public class Reaction
    {
        [Key] public int ReactionId { get; set; }
        public string ReactionText { get; set; }
<<<<<<< HEAD
=======
        [DefaultValue(false)]
        public Boolean Reported { get; set; }
>>>>>>> b39b4e77617c3cabb4d07584c7526e97422f8669
        [Required] public IdentityUser IdentityUser { get; set; }
        public Ideation Ideation { get; set; }
        public Idea Idea { get; set; }
        public ICollection<Like> Likes { get; set; }


        /*[ForeignKey("LoggedInUser")]
        public int LoggedInUserFK { get; set; }
        [ForeignKey("Ideation")]
        public int IdeationFK { get; set; }
        [ForeignKey("Idea")]
        public int IdeaFK { get; set; }*/
        public Reaction()
        {
        }

        public Reaction(Idea idea)
        {
            Idea = idea;
        }
    }
}