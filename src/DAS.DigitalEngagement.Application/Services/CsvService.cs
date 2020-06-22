using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Domain.Services;
using LINQtoCSV;

namespace DAS.DigitalEngagement.Application.Services
{
    public class CsvService : ICsvService
    {
        public async Task<List<T>> ConvertToList<T>(Stream personCsv) where T : class, new()
        {
            CsvFileDescription inputFileDescription = new CsvFileDescription
            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = true,
                FileCultureName = "en-gb",
                IgnoreUnknownColumns = true
            };

            CsvContext cc = new CsvContext();


            using (StreamReader sr = new StreamReader(personCsv))
            {
                return cc.Read<T>(sr, inputFileDescription).ToList();
            }

        }

        public byte[] ToCsv<T>(IList<T> leads)
        {
            CsvFileDescription inputFileDescription = new CsvFileDescription
            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = true,
                FileCultureName = "en-gb"
            };

            CsvContext cc = new CsvContext();

            var memStream = new MemoryStream();
            using (StreamWriter sw = new StreamWriter(memStream))
            {
                cc.Write(leads,sw);
                sw.Flush();
                return memStream.ToArray();
            }
           
        }
    }
}