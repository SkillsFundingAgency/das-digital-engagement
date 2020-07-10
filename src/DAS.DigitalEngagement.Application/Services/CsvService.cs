using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using DAS.DigitalEngagement.Domain.Services;
using LINQtoCSV;

namespace DAS.DigitalEngagement.Application.Services
{
    public class CsvService : ICsvService
    {
        public async Task<IList<dynamic>> ConvertToList(Stream personCsv)
        {


            TextReader tr = new StreamReader(personCsv);
            using (var csv = new CsvReader(tr, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<dynamic>();
                return records.ToList<dynamic>();
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

        public string ToCsvString(IList<dynamic> leads)
        {

            using (var writer = new StringWriter())
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(leads);
                
                writer.Flush();
                return writer.ToString();
            }


        }
    }
}