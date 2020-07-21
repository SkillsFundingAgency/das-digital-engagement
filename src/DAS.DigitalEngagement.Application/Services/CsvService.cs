using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using DAS.DigitalEngagement.Domain.Services;
using DAS.DigitalEngagement.Models;
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

        public int GetByteCount<T>(IList<T> leads)
        {
            var csvString = ToCsv(leads);

            return System.Text.Encoding.Unicode.GetByteCount(csvString);
        }

        public string ToCsv<T>(IList<T> leads)
        {

            using (var writer = new StringWriter())
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Configuration.RegisterClassMap<PersonMap>();

                csv.WriteRecords(leads);

                writer.Flush();
                return writer.ToString();
            }
        }
        
        private sealed class PersonMap : ClassMap<Person>
        {
            public PersonMap()
            {
                AutoMap(CultureInfo.InvariantCulture);
                Map(m => m.EmployerUserId).Name("esfaEmployerUserId");
            }
        }
    }
}