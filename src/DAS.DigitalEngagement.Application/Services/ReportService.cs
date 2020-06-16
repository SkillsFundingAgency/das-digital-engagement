using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Domain.Services;
using DAS.DigitalEngagement.Models.Marketo;
using LINQtoCSV;

namespace DAS.DigitalEngagement.Application.Services
{
    public class ReportService : IReportService
    {
        public string CreateImportReport(IList<BulkImportJob> importJobs)
        {
            var sb = new StringBuilder();
            sb.Append($"################################################################################").AppendLine();
            sb.Append($"#################### Marketo bulk person import report #########################").AppendLine();
            sb.Append($"################################################################################").AppendLine().AppendLine();
            sb.Append($"Import time: {DateTime.Now}").AppendLine().AppendLine();

            sb.Append(
                    $"{importJobs.Count} jobs have been queued for import into marketo. Please see the status of each import job below:")
                .AppendLine().AppendLine();

            for (int i = 0; i < importJobs.Count; i++)
            {
                var job = importJobs[i];
                sb.Append($"################################################################################").AppendLine();
                sb.Append($"Bulk import job {i+1} of {importJobs.Count} details:").AppendLine();
                sb.Append(job.ToString());
            }
            sb.Append($"################################################################################");
            return sb.ToString();
        }
    }
}