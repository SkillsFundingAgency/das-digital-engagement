using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using DAS.DigitalEngagement.Models.DataCollection;
using DAS.DigitalEngagement.Models.Infrastructure;
using Newtonsoft.Json;

namespace DAS.DigitalEngagement.Models.Marketo
{
    /// <summary>
    /// PushLeadToMarketoRequest
    /// </summary>
    [DataContract]
    public partial class PushLeadToMarketoRequest :  IEquatable<PushLeadToMarketoRequest>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PushLeadToMarketoRequest" /> class.
        /// </summary>
        /// <param name="input">input.</param>
        /// <param name="lookupField">lookupField.</param>
        /// <param name="partitionName">partitionName.</param>
        /// <param name="programName">programName.</param>
        /// <param name="programStatus">programStatus.</param>
        /// <param name="reason">reason.</param>
        /// <param name="source">source.</param>
        public PushLeadToMarketoRequest(List<NewLead> input = default(List<NewLead>), string lookupField = default(string), string partitionName = default(string), string programName = default(string), string programStatus = default(string), string reason = default(string), string source = default(string))
        {
            this.Input = input;
            this.LookupField = lookupField;
            this.PartitionName = partitionName;
            this.ProgramName = programName;
            this.ProgramStatus = programStatus;
            this.Reason = reason;
            this.Source = source;
        }
        
        /// <summary>
        /// Gets or Sets Input
        /// </summary>
        [DataMember(Name="input", EmitDefaultValue=false)]
        public List<NewLead> Input { get; set; }

        /// <summary>
        /// Gets or Sets LookupField
        /// </summary>
        [DataMember(Name="lookupField", EmitDefaultValue=false)]
        public string LookupField { get; set; }

        /// <summary>
        /// Gets or Sets PartitionName
        /// </summary>
        [DataMember(Name="partitionName", EmitDefaultValue=false)]
        public string PartitionName { get; set; }

        /// <summary>
        /// Gets or Sets ProgramName
        /// </summary>
        [DataMember(Name="programName", EmitDefaultValue=false)]
        public string ProgramName { get; set; }

        /// <summary>
        /// Gets or Sets ProgramStatus
        /// </summary>
        [DataMember(Name="programStatus", EmitDefaultValue=false)]
        public string ProgramStatus { get; set; }

        /// <summary>
        /// Gets or Sets Reason
        /// </summary>
        [DataMember(Name="reason", EmitDefaultValue=false)]
        public string Reason { get; set; }

        /// <summary>
        /// Gets or Sets Source
        /// </summary>
        [DataMember(Name="source", EmitDefaultValue=false)]
        public string Source { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class PushLeadToMarketoRequest {\n");
            sb.Append("  Input: ").Append(Input).Append("\n");
            sb.Append("  LookupField: ").Append(LookupField).Append("\n");
            sb.Append("  PartitionName: ").Append(PartitionName).Append("\n");
            sb.Append("  ProgramName: ").Append(ProgramName).Append("\n");
            sb.Append("  ProgramStatus: ").Append(ProgramStatus).Append("\n");
            sb.Append("  Reason: ").Append(Reason).Append("\n");
            sb.Append("  Source: ").Append(Source).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as PushLeadToMarketoRequest);
        }

        /// <summary>
        /// Returns true if PushLeadToMarketoRequest instances are equal
        /// </summary>
        /// <param name="input">Instance of PushLeadToMarketoRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(PushLeadToMarketoRequest input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Input == input.Input ||
                    this.Input != null &&
                    input.Input != null &&
                    this.Input.SequenceEqual(input.Input)
                ) && 
                (
                    this.LookupField == input.LookupField ||
                    (this.LookupField != null &&
                    this.LookupField.Equals(input.LookupField))
                ) && 
                (
                    this.PartitionName == input.PartitionName ||
                    (this.PartitionName != null &&
                    this.PartitionName.Equals(input.PartitionName))
                ) && 
                (
                    this.ProgramName == input.ProgramName ||
                    (this.ProgramName != null &&
                    this.ProgramName.Equals(input.ProgramName))
                ) && 
                (
                    this.ProgramStatus == input.ProgramStatus ||
                    (this.ProgramStatus != null &&
                    this.ProgramStatus.Equals(input.ProgramStatus))
                ) && 
                (
                    this.Reason == input.Reason ||
                    (this.Reason != null &&
                    this.Reason.Equals(input.Reason))
                ) && 
                (
                    this.Source == input.Source ||
                    (this.Source != null &&
                    this.Source.Equals(input.Source))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.Input != null)
                    hashCode = hashCode * 59 + this.Input.GetHashCode();
                if (this.LookupField != null)
                    hashCode = hashCode * 59 + this.LookupField.GetHashCode();
                if (this.PartitionName != null)
                    hashCode = hashCode * 59 + this.PartitionName.GetHashCode();
                if (this.ProgramName != null)
                    hashCode = hashCode * 59 + this.ProgramName.GetHashCode();
                if (this.ProgramStatus != null)
                    hashCode = hashCode * 59 + this.ProgramStatus.GetHashCode();
                if (this.Reason != null)
                    hashCode = hashCode * 59 + this.Reason.GetHashCode();
                if (this.Source != null)
                    hashCode = hashCode * 59 + this.Source.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }

        public PushLeadToMarketoRequest MapFromUserData(UserData user, RegisterInterestProgramConfiguration programConfiguration)
        {
            var newLeadRequest = new PushLeadToMarketoRequest();

            newLeadRequest.ProgramName = programConfiguration.ProgramName;
            newLeadRequest.Source = programConfiguration.Source;
            newLeadRequest.Reason = user.RouteId == "1" ? programConfiguration.CitizenReason : programConfiguration.EmployerReason;
            newLeadRequest.LookupField = programConfiguration.LookupField;

            newLeadRequest.Input = new List<NewLead>();

            var newLead = new NewLead()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IncludeInUR = user.IncludeInUR
            };

            newLeadRequest.Input.Add(newLead);

            return newLeadRequest;
        }
    }

}
