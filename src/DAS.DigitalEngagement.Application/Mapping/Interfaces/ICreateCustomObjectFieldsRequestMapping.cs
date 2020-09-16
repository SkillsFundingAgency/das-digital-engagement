using System.Collections.Generic;
using DAS.DigitalEngagement.Domain.DataModel;
using Das.Marketo.RestApiClient.Models.Requests;

namespace DAS.DigitalEngagement.Application.Mapping.Interfaces
{
    public interface ICreateCustomObjectFieldsRequestMapping
    {
        CreateCustomObjectFieldsRequest Map(IList<Fields> fields);
    }
}