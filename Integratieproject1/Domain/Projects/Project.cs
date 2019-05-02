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
        public string Logo { get; set; }
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
    }
}