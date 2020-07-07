using System.Collections.Generic;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Models.BulkImport;
using Das.Marketo.RestApiClient.Models;

namespace DAS.DigitalEngagement.Domain.DataCollection
{
    public interface IBulkImportService
    {
        Task<BulkImportJob> ImportPeople(IList<dynamic> leads);
        Task<BulkImportJob> ImportToCampaign(IList<dynamic> leads, string campaginId);
        Task<BulkImportStatus> GetJobStatus(int jobId);
        Task<string> GetWarnings(int jobId);
        Task<string> GetFailures(int jobId);
    }
}
