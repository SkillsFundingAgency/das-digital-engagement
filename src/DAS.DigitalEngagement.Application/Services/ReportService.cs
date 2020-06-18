using System.Text;
using DAS.DigitalEngagement.Domain.Services;
using DAS.DigitalEngagement.Models.BulkImport;
using Das.Marketo.RestApiClient.Models;

namespace DAS.DigitalEngagement.Application.Services
{
    public class ReportService : IReportService
    {
        public string CreateImportReport(BulkImportFileStatus importFileStatus)
        {
            var sb = new StringBuilder();
            sb.Append($"################################################################################").AppendLine();
            sb.Append($"#################### Marketo bulk person import report #########################").AppendLine();
            sb.Append($"################################################################################").AppendLine().AppendLine();
            sb.Append($"Import time: {importFileStatus.StartTime}").AppendLine();
            sb.Append($"Import duration: {importFileStatus.Duration}ms").AppendLine().AppendLine();

            sb.Append(
                    $"{importFileStatus.BulkImportJobs.Count} jobs have been queued for import into marketo. Please see the status of each import job below:")
                .AppendLine().AppendLine();

            for (int i = 0; i < importFileStatus.BulkImportJobs.Count; i++)
            {
                var job = importFileStatus.BulkImportJobs[i];
                sb.Append($"################################################################################").AppendLine();
                sb.Append($"Bulk import job {i+1} of {importFileStatus.BulkImportJobs.Count} details:").AppendLine();
                sb.Append(job.ToString());
            }
            sb.Append($"################################################################################");
            return sb.ToString();
        }
    }
}