using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Refit;

namespace DAS.DigitalEngagement.Infrastructure.Repositories
{
   public interface IGithubRepository
   {
       [Get("/{fileUri}")]
       Task<string> GetFile(string fileUri);

   }
}
