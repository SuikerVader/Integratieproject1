﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Surveys;

namespace Integratieproject1.Domain.Projects
{
    public class Phase : IValidatableObject
    {
        [Key] public int PhaseId { get; set; }
        public int PhaseNr { get; set; }
        public string PhaseName { get; set; }
        public string Description { get; set; }
        [Required] public DateTime StartDate { get; set; }
        [Required] public DateTime EndDate { get; set; }
        public Project Project { get; set; }

        public ICollection<Ideation> Ideations { get; set; }
        public ICollection<Survey> Surveys { get; set; }

        public Ideation GetTopIdeation()
        {
            Ideation returnIdeation = Ideations.First();
            int returnIdeaCount = 1;
            foreach (var ideation in Ideations)
            {
               int ideaCount = 0;
               foreach (var idea in ideation.Ideas)
               {
                   ideaCount++;
               }

               if (ideaCount > returnIdeaCount)
               {
                   returnIdeation = ideation;
                   returnIdeaCount = ideaCount;
               }
            }

            return returnIdeation;
        }

        public Survey GetFirstSurvey()
        {
            return Surveys.First();
        }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            if (EndDate <= StartDate)
            {
                errors.Add(new ValidationResult("EndDate cant be before startdate",
                    new string[] {"EndDate", "StartDate"}));
            }

            return errors;
        }
    }
}