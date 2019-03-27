using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Surveys;

namespace Integratieproject1.Domain.Projects
{
    public class Phase : IValidatableObject
    {
      [Key]
        public int PhaseId { get; set; }
    //[Required]
        public int PhaseNr { get; set; }
        public string PhaseName { get; set; }
        public string Description { get; set; }
    [Required]
        public DateTime StartDate { get; set; }
    [Required]
        public DateTime EndDate { get; set; }

    //[Required]
    public Project Project { get; set; }

    public ICollection<Ideation> Ideations { get; set; }
    public ICollection<Survey> Surveys { get; set; }


    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> errors = new List<ValidationResult>();
        if (EndDate <= StartDate)
        {
            errors.Add(new ValidationResult("EndDate cant be before startdate", new  string[] {"EndDate","StartDate"}));
        }

        /*if (StartDate < Project.StartDate)
        {
            errors.Add(new ValidationResult("StartDate cant be before startdate of the project ",new string[]{"StartDate","Project.StartDate"}));
        }
        if (EndDate < Project.EndDate)
        {
            errors.Add(new ValidationResult("StartDate cant be before startdate of the project ",new string[]{"StartDate","Project.StartDate"}));
        }
        IList<Phase> phases = Project.Phases.ToList();
        foreach (var phase in phases)
        {
            if (PhaseNr > phase.PhaseNr)
            {
                if (StartDate < phase.EndDate)
                {
                    errors.Add(new ValidationResult("StartDate has to be after the previous phase",new string[]{"StartDate","phase.EndDate"}));
                }
            }
            else
            {
                if (EndDate < phase.StartDate)
                {
                    errors.Add(new ValidationResult("EndDate has to be before the next phase",new string[]{"EndDate","phase.StartDate"}));

                }
            }
            
        }*/
        return errors;
    }
    }
}
