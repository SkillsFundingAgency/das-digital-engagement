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
using DAS.DigitalEngagement.Models.Validation;
using FormatValidator;

namespace DAS.DigitalEngagement.Application.Services
{
    public class CsvService : ICsvService
    {
        public CsvValidationeResult Validate(Stream csvStream, IList<string> fields)
        {
            var config = new ValidatorConfiguration();

            config.ColumnSeperator = ",";
            config.HasHeaderRow = true;
            config.RowSeperator = Environment.NewLine;

            for (int i = 0; i < fields.Count; i++)
            {
                config.Columns.Add(i,new ColumnValidatorConfiguration()
                {
                    Name = fields[i],
                    IsNumeric = false,
                    Unique = true
                });
            }

            Validator validator = Validator.FromConfiguration(config);
            var sourceReader = new StreamSourceReader(csvStream);

            var validationResult = new CsvValidationeResult
            {
                Errors = validator.Validate(sourceReader).ToList()
            };




            return validationResult;
            }

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