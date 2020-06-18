using System.Text;

namespace DAS.DigitalEngagement.Models.Marketo
{
    public class BulkImportJob
    {
        public int batchId { get; set; }
        public string ImportId { get; set; }
        public string Status { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("BulkImportJob {\n");
            sb.Append("  BatchId: ").Append(batchId).Append("\n");
            sb.Append("  ImportId: ").Append(ImportId).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    public enum ImportStatus
    {
        Queued = 0,
        Processing = 1,
        Completed = 2,
        Failed = 3
    }
}