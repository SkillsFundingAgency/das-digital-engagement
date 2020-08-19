using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Das.Marketo.RestApiClient.Models
{
    /// <summary>
    /// LeadAttribute
    /// </summary>
    [DataContract]
    public partial class LeadAttribute :  IEquatable<LeadAttribute>, IValidatableObject
    {
        /// <summary>
        /// Datatype of the field
        /// </summary>
        /// <value>Datatype of the field</value>
        [DataMember(Name="dataType", EmitDefaultValue=false)]
        public string DataType { get; set; }

        /// <summary>
        /// UI display-name of the field
        /// </summary>
        /// <value>UI display-name of the field</value>
        [DataMember(Name="displayName", EmitDefaultValue=false)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Unique integer id of the field
        /// </summary>
        /// <value>Unique integer id of the field</value>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public int Id { get; set; }

        /// <summary>
        /// Max length of the field.  Only applicable to text, string, and text area.
        /// </summary>
        /// <value>Max length of the field.  Only applicable to text, string, and text area.</value>
        [DataMember(Name="length", EmitDefaultValue=false)]
        public int Length { get; set; }

        /// <summary>
        /// Gets or Sets Rest
        /// </summary>
        [DataMember(Name="rest", EmitDefaultValue=false)]
        public LeadMapAttribute Rest { get; set; }

        /// <summary>
        /// Gets or Sets Soap
        /// </summary>
        [DataMember(Name="soap", EmitDefaultValue=false)]
        public LeadMapAttribute Soap { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class LeadAttribute {\n");
            sb.Append("  DataType: ").Append(DataType).Append("\n");
            sb.Append("  DisplayName: ").Append(DisplayName).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Length: ").Append(Length).Append("\n");
            sb.Append("  Rest: ").Append(Rest).Append("\n");
            sb.Append("  Soap: ").Append(Soap).Append("\n");
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
            return this.Equals(input as LeadAttribute);
        }

        /// <summary>
        /// Returns true if LeadAttribute instances are equal
        /// </summary>
        /// <param name="input">Instance of LeadAttribute to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(LeadAttribute input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.DataType == input.DataType ||
                    (this.DataType != null &&
                    this.DataType.Equals(input.DataType))
                ) && 
                (
                    this.DisplayName == input.DisplayName ||
                    (this.DisplayName != null &&
                    this.DisplayName.Equals(input.DisplayName))
                ) && 
                (
                    this.Id == input.Id ||
                    this.Id.Equals(input.Id)
                ) && 
                (
                    this.Length == input.Length ||
                    this.Length.Equals(input.Length)
                ) && 
                (
                    this.Rest == input.Rest ||
                    (this.Rest != null &&
                    this.Rest.Equals(input.Rest))
                ) && 
                (
                    this.Soap == input.Soap ||
                    (this.Soap != null &&
                    this.Soap.Equals(input.Soap))
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
                if (this.DataType != null)
                    hashCode = hashCode * 59 + this.DataType.GetHashCode();
                if (this.DisplayName != null)
                    hashCode = hashCode * 59 + this.DisplayName.GetHashCode();
                hashCode = hashCode * 59 + this.Id.GetHashCode();
                hashCode = hashCode * 59 + this.Length.GetHashCode();
                if (this.Rest != null)
                    hashCode = hashCode * 59 + this.Rest.GetHashCode();
                if (this.Soap != null)
                    hashCode = hashCode * 59 + this.Soap.GetHashCode();
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
