using System;
using System.Collections.Generic;
using System.Text;

namespace DAS.DigitalEngagement.Domain.Services
{
    public interface IChunkingService
    {
        IEnumerable<IList<T>> GetChunks<T>(long totalSize, List<T> items);
    }
}
