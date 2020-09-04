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
        public CsvValidationeResult Validate(StreamReader csvStream, IList<string> fields)
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
            var sourceReader = new StreamSourceReader(csvStream.BaseStream);

            var validationResult = new CsvValidationeResult
            {
                Errors = validator.Validate(sourceReader).ToList()
            };




            return validationResult;
            }

        public async Task<IList<dynamic>> ConvertToList(StreamReader personCsv)
        {
            personCsv.DiscardBufferedData();
            personCsv.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);

            using (var csv = new CsvReader(personCsv, CultureInfo.InvariantCulture))
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

        public bool IsEmpty(StreamReader stream)
        {
            
            stream.DiscardBufferedData();
            stream.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);

            if (stream.BaseStream.Length < 2)
            {
                return true;
            }

            return String.IsNullOrWhiteSpace(stream.Peek().ToString());
        }

        public bool HasData(StreamReader stream)
        {
            stream.DiscardBufferedData();
            stream.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
            stream.ReadLine();

            //if there is data and not just headers, the second line should have data and shouldnt be whitespace

            var secondLine = stream.ReadLine();

            return String.IsNullOrWhiteSpace(secondLine) == false;
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