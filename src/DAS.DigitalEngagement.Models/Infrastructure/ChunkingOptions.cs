namespace DAS.DigitalEngagement.Models.Infrastructure
{
    public class ChunkingOptions
    {
        public const int BytesInKB = 1000;
        public double maxDensity { get; set; } = 0.95;
    }
}