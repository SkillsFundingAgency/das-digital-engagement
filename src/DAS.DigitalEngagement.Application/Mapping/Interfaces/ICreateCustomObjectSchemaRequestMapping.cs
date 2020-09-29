using DAS.DigitalEngagement.Domain.DataModel;
using Das.Marketo.RestApiClient.Models.Requests;

namespace DAS.DigitalEngagement.Application.Mapping.Interfaces
{
    public interface ICreateCustomObjectSchemaRequestMapping
    {
        CreateCustomObjectSchemaRequest Map(Table table);
    }
}