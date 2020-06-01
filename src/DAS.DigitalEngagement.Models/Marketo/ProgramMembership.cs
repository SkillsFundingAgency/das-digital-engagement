using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace DAS.DigitalEngagement.Models.Marketo
{
    /// <summary>
    /// ProgramMembership
    /// </summary>
    [DataContract]
    public partial class ProgramMembership :  IEquatable<ProgramMembership>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramMembership" /> class.
        /// </summary>
        [JsonConstructor]
        protected ProgramMembership() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramMembership" /> class.
        /// </summary>
        /// <param name="acquiredBy">Whether the lead was acquired by the parent program.</param>
        /// <param name="isExhausted">Whether the lead is currently exhausted in the stream, if applicable.</param>
        /// <param name="membershipDate">Date the lead first became a member of the program (required).</param>
        /// <param name="nurtureCadence">Cadence of the parent stream if applicable.</param>
        /// <param name="progressionStatus">Program status of the lead in the parent program (required).</param>
        /// <param name="reachedSuccess">Whether the lead is in a success-status in the parent program.</param>
        /// <param name="stream">Stream that the lead is a member of, if the parent program is an engagement program.</param>
        public ProgramMembership(bool acquiredBy = default(bool), bool isExhausted = default(bool), string membershipDate = default(string), string nurtureCadence = default(string), string progressionStatus = default(string), bool reachedSuccess = default(bool), string stream = default(string))
        {
            // to ensure "membershipDate" is required (not null)
            if (membershipDate == null)
            {
                throw new InvalidDataException("membershipDate is a required property for ProgramMembership and cannot be null");
            }
            else
            {
                this.MembershipDate = membershipDate;
            }

            // to ensure "progressionStatus" is required (not null)
            if (progressionStatus == null)
            {
                throw new InvalidDataException("progressionStatus is a required property for ProgramMembership and cannot be null");
            }
            else
            {
                this.ProgressionStatus = progressionStatus;
            }

            this.AcquiredBy = acquiredBy;
            this.IsExhausted = isExhausted;
            this.NurtureCadence = nurtureCadence;
            this.ReachedSuccess = reachedSuccess;
            this.Stream = stream;
        }
        
        /// <summary>
        /// Whether the lead was acquired by the parent program
        /// </summary>
        /// <value>Whether the lead was acquired by the parent program</value>
        [DataMember(Name="acquiredBy", EmitDefaultValue=false)]
        public bool AcquiredBy { get; set; }

        /// <summary>
        /// Whether the lead is currently exhausted in the stream, if applicable
        /// </summary>
        /// <value>Whether the lead is currently exhausted in the stream, if applicable</value>
        [DataMember(Name="isExhausted", EmitDefaultValue=false)]
        public bool IsExhausted { get; set; }

        /// <summary>
        /// Date the lead first became a member of the program
        /// </summary>
        /// <value>Date the lead first became a member of the program</value>
        [DataMember(Name="membershipDate", EmitDefaultValue=false)]
        public string MembershipDate { get; set; }

        /// <summary>
        /// Cadence of the parent stream if applicable
        /// </summary>
        /// <value>Cadence of the parent stream if applicable</value>
        [DataMember(Name="nurtureCadence", EmitDefaultValue=false)]
        public string NurtureCadence { get; set; }

        /// <summary>
        /// Program status of the lead in the parent program
        /// </summary>
        /// <value>Program status of the lead in the parent program</value>
        [DataMember(Name="progressionStatus", EmitDefaultValue=false)]
        public string ProgressionStatus { get; set; }

        /// <summary>
        /// Whether the lead is in a success-status in the parent program
        /// </summary>
        /// <value>Whether the lead is in a success-status in the parent program</value>
        [DataMember(Name="reachedSuccess", EmitDefaultValue=false)]
        public bool ReachedSuccess { get; set; }

        /// <summary>
        /// Stream that the lead is a member of, if the parent program is an engagement program
        /// </summary>
        /// <value>Stream that the lead is a member of, if the parent program is an engagement program</value>
        [DataMember(Name="stream", EmitDefaultValue=false)]
        public string Stream { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ProgramMembership {\n");
            sb.Append("  AcquiredBy: ").Append(AcquiredBy).Append("\n");
            sb.Append("  IsExhausted: ").Append(IsExhausted).Append("\n");
            sb.Append("  MembershipDate: ").Append(MembershipDate).Append("\n");
            sb.Append("  NurtureCadence: ").Append(NurtureCadence).Append("\n");
            sb.Append("  ProgressionStatus: ").Append(ProgressionStatus).Append("\n");
            sb.Append("  ReachedSuccess: ").Append(ReachedSuccess).Append("\n");
            sb.Append("  Stream: ").Append(Stream).Append("\n");
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
            return this.Equals(input as ProgramMembership);
        }

        /// <summary>
        /// Returns true if ProgramMembership instances are equal
        /// </summary>
        /// <param name="input">Instance of ProgramMembership to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ProgramMembership input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.AcquiredBy == input.AcquiredBy ||
                    this.AcquiredBy.Equals(input.AcquiredBy)
                ) && 
                (
                    this.IsExhausted == input.IsExhausted ||
                    this.IsExhausted.Equals(input.IsExhausted)
                ) && 
                (
                    this.MembershipDate == input.MembershipDate ||
                    (this.MembershipDate != null &&
                    this.MembershipDate.Equals(input.MembershipDate))
                ) && 
                (
                    this.NurtureCadence == input.NurtureCadence ||
                    (this.NurtureCadence != null &&
                    this.NurtureCadence.Equals(input.NurtureCadence))
                ) && 
                (
                    this.ProgressionStatus == input.ProgressionStatus ||
                    (this.ProgressionStatus != null &&
                    this.ProgressionStatus.Equals(input.ProgressionStatus))
                ) && 
                (
                    this.ReachedSuccess == input.ReachedSuccess ||
                    this.ReachedSuccess.Equals(input.ReachedSuccess)
                ) && 
                (
                    this.Stream == input.Stream ||
                    (this.Stream != null &&
                    this.Stream.Equals(input.Stream))
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
                hashCode = hashCode * 59 + this.AcquiredBy.GetHashCode();
                hashCode = hashCode * 59 + this.IsExhausted.GetHashCode();
                if (this.MembershipDate != null)
                    hashCode = hashCode * 59 + this.MembershipDate.GetHashCode();
                if (this.NurtureCadence != null)
                    hashCode = hashCode * 59 + this.NurtureCadence.GetHashCode();
                if (this.ProgressionStatus != null)
                    hashCode = hashCode * 59 + this.ProgressionStatus.GetHashCode();
                hashCode = hashCode * 59 + this.ReachedSuccess.GetHashCode();
                if (this.Stream != null)
                    hashCode = hashCode * 59 + this.Stream.GetHashCode();
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
    }

}
