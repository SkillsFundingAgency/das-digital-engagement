using System.Collections.Generic;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Models.DataCollection;
using DAS.DigitalEngagement.Models.Marketo;

namespace DAS.DigitalEngagement.Domain.DataCollection
{
    public interface IMarketoBulkImportService
    {
        Task<BulkImportJob> ImportLeads(IList<NewLead> leads);
    }
}
