using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAS.DigitalEngagement.Application.Mapping.Interfaces;
using DAS.DigitalEngagement.Domain.DataModel;
using Das.Marketo.RestApiClient.Models.Requests;

namespace DAS.DigitalEngagement.Application.Mapping
{
    public class CreateCustomObjectFieldsRequestMapping : ICreateCustomObjectFieldsRequestMapping
    {
        public CreateCustomObjectFieldsRequest Map(IList<Fields> fields)
        {
            var customObjectFieldsRequest = new CreateCustomObjectFieldsRequest();


            customObjectFieldsRequest.Input = fields.Select(s => new CustomObjectFields()
            {
                Description = s.description,
                DataType = s.dataType,
                DisplayName = s.displayName,
                IsDedupeField = s.isDedupeField,
                Name = s.name,
                RelatedTo = s.relatedTo == null ? null : new Relatedto() { Field = s.relatedTo.field, Name = s.relatedTo.name }
            }).ToList();

            return customObjectFieldsRequest;
        }
    }
}
