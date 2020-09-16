using System.Collections.Generic;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Domain.DataModel;
using DAS.DigitalEngagement.Models.BulkImport;
using DAS.DigitalEngagement.Models.Validation;

namespace DAS.DigitalEngagement.Domain.DataCollection
{
    public interface IDataModelConfigurationService
    {
        Task ConfigureTable(Table tableDefinition);
    }
}
