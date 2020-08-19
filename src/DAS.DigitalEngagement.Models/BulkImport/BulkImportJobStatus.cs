using System.Linq;
using System.Text;
using Das.Marketo.RestApiClient.Models;

namespace DAS.DigitalEngagement.Models.BulkImport
{
    public class BulkImportJobStatus
    {
        public int Id { get; set; }
        public string ImportId { get; set; }
        public string Status { get; set; }
        public int NumOfLeadsProcessed { get; set; }
        public int NumOfRowsFailed { get; set; }
        public int NumOfRowsWithWarning { get; set; }
        public string Message { get; set; }
        public string Failures { get; set; }
        public string Warnings { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("BulkImportStatus {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  ImportId: ").Append(ImportId).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  NumOfLeadsProcessed: ").Append(NumOfLeadsProcessed).Append("\n");
            sb.Append("  NumOfRowsFailed: ").Append(NumOfRowsFailed).Append("\n");
            sb.Append("  NumOfRowsWithWarning: ").Append(NumOfRowsWithWarning).Append("\n");
            sb.Append("  Message: ").Append(Message).Append("\n");
            sb.Append("  Failures: \n");
            sb.Append("  {").AppendLine().Append(Failures).AppendLine();
            sb.Append("  }\n");
            sb.Append("  Warnings: \n");
            sb.Append("  {").AppendLine().Append(Warnings).AppendLine();
            sb.Append("  }\n");

            return sb.ToString();
        }
    }
}


