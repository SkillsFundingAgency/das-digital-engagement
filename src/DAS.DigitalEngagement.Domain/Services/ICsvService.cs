using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Models.Marketo;

namespace DAS.DigitalEngagement.Domain.Services
{
    public interface ICsvService
    {
        Task<List<T>> ConvertToList<T>(Stream personCsv) where T : class, new();
        byte[] ToCsv<T>(IList<T> leads);
    }
}