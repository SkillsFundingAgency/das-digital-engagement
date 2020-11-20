using System;
using System.Net.Http;
using System.Threading.Tasks;
using Das.Marketo.RestApiClient.Models;
using Das.Marketo.RestApiClient.Models.Requests;
using Refit;

namespace Das.Marketo.RestApiClient.Interfaces
{
    [Headers("Authorization: Bearer","Content-Type: application/json")]
    public interface IMarketoCustomObjectSchemaClient
    {
        [Get("/customobjects/schema/{apiName}/describe.json")]
        Task<Response<CustomObjectResponse>> GetCustomObjectSchema([AliasAs("apiName")] string name);

        [Post("/customobjects/schema.json")]
        Task<ResponseWithoutResult> PushCustomObjectSchema([Body(buffered: true)] CreateCustomObjectSchemaRequest customObjectSchema);

        [Post("/customobjects/schema/{apiName}/approve.json")]
        Task<ResponseWithoutResult> ApproveCustomObjectSchema([AliasAs("apiName")] string name);

        [Post("/customobjects/schema/{apiName}/addField.json")]
        Task<ResponseWithoutResult> PushCustomObjectFields([AliasAs("apiName")] string name, [Body(buffered: true)] CreateCustomObjectFieldsRequest fields);

    }
}
