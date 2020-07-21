using System.Text;
using DAS.DigitalEngagement.Domain.Services;
using DAS.DigitalEngagement.Models.BulkImport;
using Das.Marketo.RestApiClient.Models;

namespace DAS.DigitalEngagement.Application.Services
{
    public class ReportService : IReportService
    {
        public string CreateImportReport(BulkImportStatus importStatus)
        {
            var sb = new StringBuilder();
            sb.Append($"################################################################################").AppendLine();
            sb.Append($"#################### Marketo bulk person import report #########################").AppendLine();
            sb.Append($"################################################################################").AppendLine().AppendLine();
            sb.Append($"Import time: {importStatus.StartTime}").AppendLine();
            sb.Append($"Import duration: {importStatus.Duration}ms").AppendLine().AppendLine();

            sb.Append(
                    $"{importStatus.BulkImportJobs.Count} jobs have been queued for import into marketo. Please see the status of each import job below:")
                .AppendLine().AppendLine();

            if (importStatus?.BulkImportJobStatus?.Count > 0)
            {
                ReportStatus(importStatus, sb);
            }
            else
            {
                ReportJobs(importStatus, sb);
            }
            sb.Append($"################################################################################");
            return sb.ToString();
        }

        private static void ReportJobs(BulkImportStatus importStatus, StringBuilder sb)
        {
            for (int i = 0; i < importStatus.BulkImportJobs.Count; i++)
            {
                var job = importStatus.BulkImportJobs[i];
                sb.Append($"################################################################################").AppendLine();
                sb.Append($"Bulk import job {i + 1} of {importStatus.BulkImportJobs.Count} details:").AppendLine();
                sb.Append(job.ToString());

                sb.Append($"################################################################################").AppendLine();
            }
        }

        private static void ReportStatus(BulkImportStatus importStatus, StringBuilder sb)
        {
            for (int i = 0; i < importStatus.BulkImportJobStatus.Count; i++)
            {
                var status = importStatus.BulkImportJobStatus[i];
                sb.Append($"################################################################################").AppendLine();
                sb.Append($"Bulk import job {i + 1} of {importStatus.BulkImportJobStatus.Count} details:").AppendLine();
                sb.Append(status.ToString());

                sb.Append($"################################################################################").AppendLine();
            }
        }
    }
}