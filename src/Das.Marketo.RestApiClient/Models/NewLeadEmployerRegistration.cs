using System;
using System.Runtime.Serialization;

namespace Das.Marketo.RestApiClient.Models
{
    [DataContract]
    public class EmployerRegistrationLead
    {
        [DataMember(Name = "email", EmitDefaultValue = false)]
        public string Email { get; set; }
        [DataMember(Name = "employerAccountId", EmitDefaultValue = false)]
        public long EmployerAccountId { get; set; }
        [DataMember(Name = "stageCompleted", EmitDefaultValue = false)]
        public int StageCompleted { get; set; }
        [DataMember(Name = "totalStages", EmitDefaultValue = false)]
        public int TotalStages { get; set; }
        [DataMember(Name = "dateOfEvent", EmitDefaultValue = false)]
        public DateTime DateOfEvent { get; set; }
    }

}
