namespace DAS.DigitalEngagement.Models.Infrastructure
{
    public class ChunkingOptions
    {
        public int maxSize { get; set; } = 10;
        public double maxDensity { get; set; } = 0.95;
    }
}