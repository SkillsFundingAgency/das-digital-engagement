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

            if (importFileStatus?.BulkImportStatus?.Count > 0)
            {
                ReportStatus(importFileStatus, sb);
            }
            else
            {
                ReportJobs(importFileStatus, sb);
            }
            sb.Append($"################################################################################");
            return sb.ToString();
        }

        private static void ReportJobs(BulkImportFileStatus importFileStatus, StringBuilder sb)
        {
            for (int i = 0; i < importFileStatus.BulkImportJobs.Count; i++)
            {
                var job = importFileStatus.BulkImportJobs[i];
                sb.Append($"################################################################################").AppendLine();
                sb.Append($"Bulk import job {i + 1} of {importFileStatus.BulkImportJobs.Count} details:").AppendLine();
                sb.Append(job.ToString());

                sb.Append($"################################################################################").AppendLine();
            }
        }

        private static void ReportStatus(BulkImportFileStatus importFileStatus, StringBuilder sb)
        {
            for (int i = 0; i < importFileStatus.BulkImportStatus.Count; i++)
            {
                var status = importFileStatus.BulkImportStatus[i];
                sb.Append($"################################################################################").AppendLine();
                sb.Append($"Bulk import job {i + 1} of {importFileStatus.BulkImportStatus.Count} details:").AppendLine();
                sb.Append(status.ToString());

                sb.Append($"################################################################################").AppendLine();
            }
        }
    }
}