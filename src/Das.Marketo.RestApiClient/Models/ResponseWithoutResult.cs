using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Das.Marketo.RestApiClient.Models
{
    /// <summary>
    /// ResponseWithoutResult
    /// </summary>
    [DataContract]
    public partial class ResponseWithoutResult :  IEquatable<ResponseWithoutResult>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseWithoutResult" /> class.
        /// </summary>
        [JsonConstructor]
        protected ResponseWithoutResult() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseWithoutResult" /> class.
        /// </summary>
        /// <param name="errors">Array of errors that occurred if the request was unsuccessful.</param>
        /// <param name="nextPageToken">Paging token returned from a previous response.</param>
        /// <param name="requestId">Id of the request made (required).</param>
        /// <param name="success">Whether the request succeeded (required).</param>
        /// <param name="warnings">Array of warnings given for the operation.</param>
        public ResponseWithoutResult(List<Error> errors = default(List<Error>), string nextPageToken = default(string), string requestId = default(string), bool success = default(bool), List<Warning> warnings = default(List<Warning>))
        {
            // to ensure "requestId" is required (not null)
            if (requestId == null)
            {
                throw new InvalidDataException("requestId is a required property for ResponseWithoutResult and cannot be null");
            }
            else
            {
                this.RequestId = requestId;
            }

            // to ensure "success" is required (not null)
            if (success == null)
            {
                throw new InvalidDataException("success is a required property for ResponseWithoutResult and cannot be null");
            }
            else
            {
                this.Success = success;
            }

            this.Errors = errors;
            this.NextPageToken = nextPageToken;
            this.Warnings = warnings;
        }
        
        /// <summary>
        /// Array of errors that occurred if the request was unsuccessful
        /// </summary>
        /// <value>Array of errors that occurred if the request was unsuccessful</value>
        [DataMember(Name="errors", EmitDefaultValue=false)]
        public List<Error> Errors { get; set; }

        /// <summary>
        /// Paging token returned from a previous response
        /// </summary>
        /// <value>Paging token returned from a previous response</value>
        [DataMember(Name="nextPageToken", EmitDefaultValue=false)]
        public string NextPageToken { get; set; }

        /// <summary>
        /// Id of the request made
        /// </summary>
        /// <value>Id of the request made</value>
        [DataMember(Name="requestId", EmitDefaultValue=false)]
        public string RequestId { get; set; }

        /// <summary>
        /// Whether the request succeeded
        /// </summary>
        /// <value>Whether the request succeeded</value>
        [DataMember(Name="success", EmitDefaultValue=false)]
        public bool Success { get; set; }

        /// <summary>
        /// Array of warnings given for the operation
        /// </summary>
        /// <value>Array of warnings given for the operation</value>
        [DataMember(Name="warnings", EmitDefaultValue=false)]
        public List<Warning> Warnings { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ResponseWithoutResult {\n");
            sb.Append("  Errors: ").Append(Errors.Select(s => $"{{Code:{s.Code} - Message: {s.Message}}}, \n")).Append("\n");
            sb.Append("  RequestId: ").Append(RequestId).Append("\n");
            sb.Append("  Success: ").Append(Success).Append("\n");
            sb.Append("  Warnings: ").Append(Warnings.Select(s => $"{{Code:{s.Code} - Message: {s.Message}}}, \n")).Append("\n");

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
            return this.Equals(input as ResponseWithoutResult);
        }

        /// <summary>
        /// Returns true if ResponseWithoutResult instances are equal
        /// </summary>
        /// <param name="input">Instance of ResponseWithoutResult to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ResponseWithoutResult input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Errors == input.Errors ||
                    this.Errors != null &&
                    input.Errors != null &&
                    this.Errors.SequenceEqual(input.Errors)
                ) && 
                (
                    this.NextPageToken == input.NextPageToken ||
                    (this.NextPageToken != null &&
                    this.NextPageToken.Equals(input.NextPageToken))
                ) && 
                (
                    this.RequestId == input.RequestId ||
                    (this.RequestId != null &&
                    this.RequestId.Equals(input.RequestId))
                ) && 
                (
                    this.Success == input.Success ||
                    this.Success.Equals(input.Success)
                ) && 
                (
                    this.Warnings == input.Warnings ||
                    this.Warnings != null &&
                    input.Warnings != null &&
                    this.Warnings.SequenceEqual(input.Warnings)
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
                if (this.Errors != null)
                    hashCode = hashCode * 59 + this.Errors.GetHashCode();
                if (this.NextPageToken != null)
                    hashCode = hashCode * 59 + this.NextPageToken.GetHashCode();
                if (this.RequestId != null)
                    hashCode = hashCode * 59 + this.RequestId.GetHashCode();
                hashCode = hashCode * 59 + this.Success.GetHashCode();
                if (this.Warnings != null)
                    hashCode = hashCode * 59 + this.Warnings.GetHashCode();
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
