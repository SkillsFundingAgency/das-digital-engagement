using System.IO;
using System.Threading.Tasks;
using Das.Marketo.RestApiClient.Models;

namespace DAS.DigitalEngagement.Domain.Import
{
    public interface IImportPersonHandler
    {
        Task<BulkImportFileStatus> Handle(Stream personCsv);
    }
}