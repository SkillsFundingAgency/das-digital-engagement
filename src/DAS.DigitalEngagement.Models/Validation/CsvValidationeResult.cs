using System.Collections.Generic;
using System.Linq;
using FormatValidator;

namespace DAS.DigitalEngagement.Models.Validation
{
    public class CsvValidationeResult
    {
        public CsvValidationeResult()
        {
            Errors = new List<RowValidationError>();
        }
        public bool IsValid => Errors.Any() == false;
        public IList<RowValidationError> Errors { get; set; }
    }
}