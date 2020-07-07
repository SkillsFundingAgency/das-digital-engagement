using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DAS.DigitalEngagement.Domain.Services
{
    public interface ICsvService
    {
        Task<IList<dynamic>> ConvertToList(Stream personCsv);
        byte[] ToCsv<T>(IList<T> leads);
        string ToCsvString(IList<dynamic> leads);
    }
}