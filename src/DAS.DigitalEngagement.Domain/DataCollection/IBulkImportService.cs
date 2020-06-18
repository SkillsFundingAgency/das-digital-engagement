using System.Collections.Generic;
using System.Threading.Tasks;
using Das.Marketo.RestApiClient.Models;

namespace DAS.DigitalEngagement.Domain.DataCollection
{
    public interface IBulkImportService
    {
        Task<BulkImportJob> ImportPeople(IList<NewLead> leads);
        Task<BulkImportStatus> GetJobStatus(int jobId);
    }
}
