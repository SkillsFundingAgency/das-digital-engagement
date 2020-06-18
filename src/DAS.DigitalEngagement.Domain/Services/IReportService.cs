using Das.Marketo.RestApiClient.Models;

namespace DAS.DigitalEngagement.Domain.Services
{
    public interface IReportService
    {
        string CreateImportReport(BulkImportFileStatus importJobs);
    }
}