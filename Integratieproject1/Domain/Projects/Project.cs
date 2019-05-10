using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Integratieproject1.Domain.Datatypes;

namespace Integratieproject1.Domain.Projects
{
    public class Project : IValidatableObject
    {
        [Key] public int ProjectId { get; set; }
        [Required] public string ProjectName { get; set; }
        [Required] public DateTime StartDate { get; set; }
        [Required] public DateTime EndDate { get; set; }
        public string Objective { get; set; }
        public string Description { get; set; }
        [Required] public Location Location { get; set; }
        public string Status { get; set; }
        public string BackgroundImage { get; set; }

        //[Required] 
        public Platform Platform { get; set; }

        public ICollection<Phase> Phases { get; set; }
        public ICollection<AdminProject> AdminProjects { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            if (EndDate <= StartDate)
            {
                errors.Add(new ValidationResult(
                    "EndDate cant be before startdate and the startdate cant be in the past",
                    new string[] {"EndDate", "StartDate"}));
            }

            return errors;
        }

        public int GetIdeaCount()
        {
            int ideaCount = 0;
            foreach (var phase in Phases)
            {
                foreach (var ideation in phase.Ideations)
                {
                    foreach (var idea in ideation.Ideas)
                    {
                        ideaCount++;
                    }
                }
            }
            return ideaCount;
        }

        public int GetVoteCount()
        {
            int voteCount = 0;
            foreach (var phase in Phases)
                        {
                            foreach (var ideation in phase.Ideations)
                            {
                                foreach (var idea in ideation.Ideas)
                                {
                                    foreach (var vote in idea.Votes)
                                    {
                                        voteCount++;
                                    }
                                }
                            }
                        }
            return voteCount;
        }

        public int GetReactionCount()
        {
            int reactionCount = 0;
            foreach (var phase in Phases)
            {
                foreach (var ideation in phase.Ideations)
                {
                    foreach (var reaction in ideation.Reactions)
                    {
                        reactionCount++;
                    }
                    foreach (var idea in ideation.Ideas)
                    {
                        foreach (var reaction in idea.Reactions)
                        {
                            reactionCount++;
                        }
                    }
                }
            }
            return reactionCount;
        }
    }
}