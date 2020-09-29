using System.Collections.Generic;

namespace DAS.DigitalEngagement.Infrastructure.Repositories
{
    public interface IDataMartRepository
    {
        IList<dynamic> RetrieveViewData(string viewName);
    }
}