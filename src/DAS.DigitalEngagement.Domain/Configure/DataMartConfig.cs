using System.Collections.Generic;

namespace DAS.DigitalEngagement.Domain.Configure
{
    public class DataMartConfig 
    {
        public IList<DataMartSettings> Settings { get; set; }
    }

    public class DataMartSettings
    {
        public string ViewName { get; set; }
        public string ObjectName { get; set; }
        public string ConfigFileLocation { get; set; }
        public string Config { get; set; }

    }
}