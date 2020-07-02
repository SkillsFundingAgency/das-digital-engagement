using System.Collections.Generic;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Models.BulkImport;
using Das.Marketo.RestApiClient.Models;

namespace DAS.DigitalEngagement.Domain.DataCollection
{
    public interface IBulkImportService
    {
        Task<BulkImportJob> ImportPeople(IList<NewLead> leads);
        Task<BulkImportJob> ImportToCampaign(IList<NewLead> leads, string campaginId);
        Task<BulkImportStatus> GetJobStatus(int jobId);
        Task<string> GetWarnings(int jobId);
        Task<string> GetFailures(int jobId);
    }
}
