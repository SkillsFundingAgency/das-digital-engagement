namespace Das.Marketo.RestApiClient.Models
{
    public class ApiResult
    {
        public string RequestId { get; set; }
        public bool Success { get; set; }
    }

    public class ApiResult<T> : ApiResult where T : class
    {
    }
}
