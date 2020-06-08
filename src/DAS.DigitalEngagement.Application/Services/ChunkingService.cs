﻿using System;
using System.Collections.Generic;
using System.Text;
using DAS.DigitalEngagement.Domain.Services;
using DAS.DigitalEngagement.Models.Infrastructure;
using Microsoft.Extensions.Options;

namespace DAS.DigitalEngagement.Application.Services
{
    public class ChunkingService : IChunkingService
    {
        public readonly ChunkingOptions _chunkingOptions;

        public ChunkingService(IOptions<ChunkingOptions> chunkingOptions)
        {
            _chunkingOptions = chunkingOptions.Value;
        }

        private int CalculateChunkSize(int itemCount, long myBlobLength)
        {
            var maxSize = _chunkingOptions.maxSize * 1000000;

            //Calculate the total number of items in a chunk. This allows for 5% just in case some items in the list are larger than the average.   
            var totalChunks = (int)Math.Ceiling(myBlobLength / (maxSize * _chunkingOptions.maxDensity));

            var chunkSize = itemCount / totalChunks;

            return chunkSize;
        }

        private IEnumerable<List<T>> SplitList<T>(List<T> locations, int nSize)
        {
            for (int i = 0; i < locations.Count; i += nSize)
            {
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
            }
        }

        public IEnumerable<IList<T>> GetChunks<T>(long totalSize, List<T> items)
        {
            var chunkSize = CalculateChunkSize(items.Count, totalSize);

            return SplitList(items, chunkSize);
        }
    }
}