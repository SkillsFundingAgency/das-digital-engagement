namespace Das.Marketo.RestApiClient.Models
{
    public class CustomObjectField
    {
        public string name { get; set; }
        public string displayName { get; set; }
        public string dataType { get; set; }
        public int length { get; set; }
        public bool updateable { get; set; }
        public bool crmManaged { get; set; }
    }
}