using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Models.Validation;

namespace DAS.DigitalEngagement.Domain.Services
{
    public interface ICsvService
    {
        CsvValidationeResult Validate(Stream csvStream, IList<string> fields);
        Task<IList<dynamic>> ConvertToList(Stream csvStream);
        int GetByteCount<T>(IList<T> leads);
        string ToCsv<T>(IList<T> leads);
    }
}