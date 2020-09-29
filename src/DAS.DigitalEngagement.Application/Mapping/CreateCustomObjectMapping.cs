using System;
using System.Collections.Generic;
using System.Text;
using DAS.DigitalEngagement.Application.Mapping.Interfaces;
using DAS.DigitalEngagement.Domain.DataModel;
using Das.Marketo.RestApiClient.Models.Requests;

namespace DAS.DigitalEngagement.Application.Mapping
{
    public class CreateCustomObjectSchemaRequestMapping : ICreateCustomObjectSchemaRequestMapping
    {
        public CreateCustomObjectSchemaRequest Map(Table table)
        {
            var schema = new CreateCustomObjectSchemaRequest()
            {
                Action = table.action,
                ApiName = table.apiName,
                Description = table.description,
                DisplayName = table.displayName
            };
            return schema;
        }
    }
}
