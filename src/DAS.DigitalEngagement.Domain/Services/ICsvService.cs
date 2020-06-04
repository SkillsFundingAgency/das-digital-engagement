using System.Collections.Generic;
using System.IO;
using DAS.DigitalEngagement.Models.Marketo;

namespace DAS.DigitalEngagement.Domain.Services
{
    public interface ICsvService
    {
        List<T> ConvertToList<T>(Stream personCsv) where T : class, new();
        byte[] ToCsv<T>(IList<T> leads);
    }
}