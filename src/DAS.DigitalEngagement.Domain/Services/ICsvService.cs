using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Models.Validation;

namespace DAS.DigitalEngagement.Domain.Services
{
    public interface ICsvService
    {
        CsvValidationeResult Validate(StreamReader csvStream, IList<string> fields);
        Task<IList<dynamic>> ConvertToList(StreamReader csvStream);
        int GetByteCount<T>(IList<T> leads);
        string ToCsv<T>(IList<T> leads);
        bool IsEmpty(StreamReader stream);
        bool HasData(StreamReader stream);
    }
}