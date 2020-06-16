using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Models.Marketo;

namespace DAS.DigitalEngagement.Domain.Import
{
    public interface IImportPersonHandler
    {
        Task<IList<BulkImportJob>> Handle(Stream personCsv);
    }
}