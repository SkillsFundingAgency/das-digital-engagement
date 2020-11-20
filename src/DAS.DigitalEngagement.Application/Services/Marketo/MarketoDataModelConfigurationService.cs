using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Application.Mapping;
using DAS.DigitalEngagement.Application.Mapping.Interfaces;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Domain.DataModel;
using Das.Marketo.RestApiClient.Interfaces;
using Das.Marketo.RestApiClient.Models;
using Microsoft.Extensions.Logging;

namespace DAS.DigitalEngagement.Application.Services.Marketo
{
    public class MarketoDataModelConfigurationService : IDataModelConfigurationService
    {

        private readonly ILogger<MarketoDataModelConfigurationService> _logger;
        private readonly IMarketoCustomObjectSchemaClient _customObjectSchemaClient;
        private readonly ICreateCustomObjectSchemaRequestMapping _customObjectSchemaRequestMapping;
        private readonly ICreateCustomObjectFieldsRequestMapping _customObjectFieldsRequestMapping;

        public MarketoDataModelConfigurationService(ILogger<MarketoDataModelConfigurationService> logger, IMarketoCustomObjectSchemaClient customObjectSchemaClient, ICreateCustomObjectSchemaRequestMapping customObjectSchemaRequestMapping, ICreateCustomObjectFieldsRequestMapping customObjectFieldsRequestMapping)
        {
            _logger = logger;
            _customObjectSchemaClient = customObjectSchemaClient;
            _customObjectSchemaRequestMapping = customObjectSchemaRequestMapping;
            _customObjectFieldsRequestMapping = customObjectFieldsRequestMapping;
        }


        public async Task ConfigureTable(Table tableDefinition)
        {
            //Try and pull back the definition of the custom object
            var customObject = await _customObjectSchemaClient.GetCustomObjectSchema(tableDefinition.apiName);
            if (null == customObject)
            {
                _logger.LogError($"Error retrieving Custom Object: {tableDefinition.apiName}");
                return;
            }

            //check if custom object exists in marketo
            if (customObject.Success && customObject.Result.FirstOrDefault().Approved != null)
            {
                _logger.LogInformation("Custom object: {tableDefinition.apiName} already exists in Marketo, and is approved, so no need to create");
                return;
            }

            _logger.LogWarning($"Custom Object: {tableDefinition.apiName} does NOT exist in Marketo or is NOT approved - so trying to create it");

            //Create custom object
            if (await CreateCustomObject(tableDefinition) == false) return;

            //Create custom object fields
            if (await CreateCustomObjectFields(tableDefinition) == false) return;

            //approve object
            await ApproveCustomObject(tableDefinition);
        }

        private async Task<bool> ApproveCustomObject(Table tableDefinition)
        {
            var customObjectSchemaResponse = await _customObjectSchemaClient.ApproveCustomObjectSchema(tableDefinition.apiName);

            if (customObjectSchemaResponse.Success == false)
            {
                _logger.LogError($"Unable to approve custom object: {tableDefinition.apiName}, Response: {customObjectSchemaResponse}");
                return false;
            }
            return true;
        }

        private async Task<bool> CreateCustomObjectFields(Table tableDefinition)
        {
            var customObjectSchemaResponse =
                await _customObjectSchemaClient.PushCustomObjectFields(tableDefinition.apiName,
                    _customObjectFieldsRequestMapping.Map(tableDefinition.fields.ToList()));

            if (customObjectSchemaResponse.Success == false)
            {
                _logger.LogError($"Unable to create custom object fields for : {tableDefinition.apiName}, Response: {customObjectSchemaResponse.ToString()}");
                return false;
            }

            return true;
        }

        private async Task<bool> CreateCustomObject(Table tableDefinition)
        {
            var customObjectSchemaResponse = await _customObjectSchemaClient.PushCustomObjectSchema(_customObjectSchemaRequestMapping.Map(tableDefinition));

            if (customObjectSchemaResponse.Success == false)
            {
                _logger.LogError($"Unable to create custom object: {tableDefinition.apiName}, Response: {customObjectSchemaResponse.ToString()}");
                return false;
            }

            return true;
        }
    }
}
