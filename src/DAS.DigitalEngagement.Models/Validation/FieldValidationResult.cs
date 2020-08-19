using System.Collections.Generic;
using System.Linq;

namespace DAS.DigitalEngagement.Models.Validation
{
    public class FieldValidationResult
    {
        public FieldValidationResult()
        {
            Errors = new List<string>();
        }
        public bool IsValid => Errors.Any() == false;
        public IList<string> Errors { get; set; }
    }
}