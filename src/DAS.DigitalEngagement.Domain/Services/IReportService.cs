using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Models.Marketo;

namespace DAS.DigitalEngagement.Domain.Services
{
    public interface IReportService
    {
        string CreateImportReport(IList<BulkImportJob> importJobs);
    }
}