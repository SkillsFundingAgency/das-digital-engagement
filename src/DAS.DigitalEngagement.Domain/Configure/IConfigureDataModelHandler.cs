using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAS.DigitalEngagement.Domain.Configure
{
    public interface IConfigureDataModelHandler
    {

        Task ConfigureDataModel(IList<DataMartSettings> dataMartSettings);
    }
}