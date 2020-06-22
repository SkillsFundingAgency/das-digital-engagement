using System.IO;
using System.Threading.Tasks;

namespace DAS.DigitalEngagement.Domain.Import
{
    public interface IImportPersonHandler
    {
        Task Handle(Stream personCsv);
    }
}