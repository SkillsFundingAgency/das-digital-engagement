using System.Collections.Generic;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Models.BulkImport;
using DAS.DigitalEngagement.Models.Validation;

namespace DAS.DigitalEngagement.Domain.DataCollection
{
    public interface IBulkImportService
    {
        Task<FieldValidationResult> ValidateFields(IList<string> fields);
        Task<BulkImportStatus> ImportPeople<T>(IList<T> leads);
        Task<BulkImportJob> ImportToCampaign<T>(IList<T> leads, string campaginId);
        Task<BulkImportJobStatus> GetJobStatus(int jobId);
        Task<string> GetWarnings(int jobId);
        Task<string> GetFailures(int jobId);
    }
}
