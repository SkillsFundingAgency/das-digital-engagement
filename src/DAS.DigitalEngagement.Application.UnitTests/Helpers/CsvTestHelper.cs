using System.Text;

namespace DAS.DigitalEngagement.Application.UnitTests.Helpers
{
    public class CsvTestHelper
    {
        public static string GetValidCsv_SingleChunk()
        {

            var csv = new StringBuilder()
                .AppendLine($"FirstName,LastName,Email,Company")
                .AppendLine($"Person,One,Person.one@email.com,CompanyOne")
                .AppendLine($"Person,Two,Person.Two@email.com,CompanyTwo")
                .AppendLine($"Person,Three,Person.Three@email.com,CompanyThree");


            return csv.ToString();
        }

        public static string GetValidCsv_AdditionalProperties()
        {

            var csv = new StringBuilder()
                .AppendLine($"FirstName,LastName,Email,Company,AdditionalColumn")
                .AppendLine($"Person,One,Person.one@email.com,CompanyOne,true")
                .AppendLine($"Person,Two,Person.Two@email.com,CompanyTwo,true")
                .AppendLine($"Person,Three,Person.Three@email.com,CompanyThree,true");


            return csv.ToString();
        }

        public static string GetValidCsv(int count, string firstname, string lastname, string company)
        {

            var csv = new StringBuilder()
                .AppendLine($"FirstName,LastName,Email,Company");

            for (int i = 1; i < count+1; i++)
            {
                
                csv.AppendLine($"{firstname}{i},{lastname},{firstname}.{lastname}.{i}@email.com,{company}{i}");
            }

            return csv.ToString();
        }
    }
}