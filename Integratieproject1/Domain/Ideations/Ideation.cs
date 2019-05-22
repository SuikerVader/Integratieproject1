using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Integratieproject1.Domain.Projects;

namespace Integratieproject1.Domain.Ideations
{
    public class Ideation
    {
        [Key] public int IdeationId { get; set; }
        [Required] public string CentralQuestion { get; set; }

        [Required] public bool InputIdeation { get; set; }
        public bool Text { get; set; } = true;
        public bool Image { get; set; } = true;
        public bool Video { get; set; } = true;
        public bool Map { get; set; } = true;
        public bool TextRequired { get; set; }
        public bool ImageRequired { get; set; }
        public bool VideoRequired { get; set; }
        public bool MapRequired { get; set; }
        public string ExternalLink { get; set; }

        //[Required] 
        public Phase Phase { get; set; }
        public ICollection<Reaction> Reactions { get; set; }
        public ICollection<Idea> Ideas { get; set; }

        public Idea GetTopIdea()
        {
            Idea returnIdea = Ideas.First();
            foreach (var idea in Ideas)
            {
                if (idea.Votes.Count > returnIdea.Votes.Count)
                {
                    returnIdea = idea;
                }
            }

          return  returnIdea;
        }
    }
}