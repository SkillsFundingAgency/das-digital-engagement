using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace DAS.DigitalEngagement.Models.Marketo
{
    /// <summary>
    /// Lead record.  Always contains id, but may have any number of other fields, depending on the fields available in the target instance.
    /// </summary>
    [DataContract]
    public partial class NewLead :  IEquatable<NewLead>, IValidatableObject
    {

        /// <summary>
        /// Unique integer id of a lead record
        /// </summary>
        /// <value>Unique integer id of a lead record</value>
        [DataMember(Name="email", EmitDefaultValue=false)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or Sets Membership
        /// </summary>
        [DataMember(Name= "firstName", EmitDefaultValue=false)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or Sets Membership
        /// </summary>
        [DataMember(Name = "lastName", EmitDefaultValue = false)]
        public string LastName { get; set; }
        
        /// <summary>
        /// Gets or Sets IncludeInUR
        /// </summary>
        [DataMember(Name = "includeInUr", EmitDefaultValue = false)]
        public bool IncludeInUR { get; set; }

        /// <summary>
        /// Gets or Sets Membership
        /// </summary>
        [DataMember(Name = "company", EmitDefaultValue = false)]
        public string Company { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Lead {\n");
            sb.Append("  Email: ").Append(Email).Append("\n");
            sb.Append("  FirstName: ").Append(FirstName).Append("\n");
            sb.Append("  LastName: ").Append(LastName).Append("\n");
            sb.Append("  IncludeInUR: ").Append(IncludeInUR).Append("\n");
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
            return this.Equals(input as NewLead);
        }

        /// <summary>
        /// Returns true if Lead instances are equal
        /// </summary>
        /// <param name="input">Instance of Lead to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(NewLead input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Email == input.Email ||
                    this.Email.Equals(input.Email)
                ) && 
                (
                    this.FirstName == input.FirstName ||
                    (this.FirstName != null &&
                    this.FirstName.Equals(input.FirstName))
                ) && 
                (
                    this.LastName == input.LastName ||
                    (this.LastName != null &&
                    this.LastName.Equals(input.LastName))
                ) && 
                (
                    this.IncludeInUR == input.IncludeInUR ||
                    this.IncludeInUR.Equals(input.IncludeInUR)
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
                hashCode = hashCode * 59 + this.Email.GetHashCode();
                if (this.FirstName != null)
                    hashCode = hashCode * 59 + this.FirstName.GetHashCode();
                if (this.LastName != null)
                    hashCode = hashCode * 59 + this.LastName.GetHashCode();
                if (this.IncludeInUR != null)
                    hashCode = hashCode * 59 + this.IncludeInUR.GetHashCode();
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
