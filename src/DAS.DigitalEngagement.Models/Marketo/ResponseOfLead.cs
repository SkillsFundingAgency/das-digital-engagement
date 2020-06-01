using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace DAS.DigitalEngagement.Models.Marketo
{
    /// <summary>
    /// ResponseOfLead
    /// </summary>
    [DataContract]
    public partial class ResponseOfLead :  IEquatable<ResponseOfLead>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseOfLead" /> class.
        /// </summary>
        [JsonConstructor]
        protected ResponseOfLead() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseOfLead" /> class.
        /// </summary>
        /// <param name="errors">Array of errors that occurred if the request was unsuccessful (required).</param>
        /// <param name="moreResult">Boolean indicating if there are more results in subsequent pages.</param>
        /// <param name="nextPageToken">Paging token given if the result set exceeded the allowed batch size.</param>
        /// <param name="requestId">Id of the request made (required).</param>
        /// <param name="result">Array of results for individual records in the operation, may be empty (required).</param>
        /// <param name="success">Whether the request succeeded (required).</param>
        /// <param name="warnings">Array of warnings given for the operation (required).</param>
        public ResponseOfLead(List<Error> errors = default(List<Error>), bool moreResult = default(bool), string nextPageToken = default(string), string requestId = default(string), List<Lead> result = default(List<Lead>), bool success = default(bool), List<Warning> warnings = default(List<Warning>))
        {
            // to ensure "errors" is required (not null)
            if (errors == null)
            {
                throw new InvalidDataException("errors is a required property for ResponseOfLead and cannot be null");
            }
            else
            {
                this.Errors = errors;
            }

            // to ensure "requestId" is required (not null)
            if (requestId == null)
            {
                throw new InvalidDataException("requestId is a required property for ResponseOfLead and cannot be null");
            }
            else
            {
                this.RequestId = requestId;
            }

            // to ensure "result" is required (not null)
            if (result == null)
            {
                throw new InvalidDataException("result is a required property for ResponseOfLead and cannot be null");
            }
            else
            {
                this.Result = result;
            }

            // to ensure "success" is required (not null)
            if (success == null)
            {
                throw new InvalidDataException("success is a required property for ResponseOfLead and cannot be null");
            }
            else
            {
                this.Success = success;
            }

            // to ensure "warnings" is required (not null)
            if (warnings == null)
            {
                throw new InvalidDataException("warnings is a required property for ResponseOfLead and cannot be null");
            }
            else
            {
                this.Warnings = warnings;
            }

            this.MoreResult = moreResult;
            this.NextPageToken = nextPageToken;
        }
        
        /// <summary>
        /// Array of errors that occurred if the request was unsuccessful
        /// </summary>
        /// <value>Array of errors that occurred if the request was unsuccessful</value>
        [DataMember(Name="errors", EmitDefaultValue=false)]
        public List<Error> Errors { get; set; }

        /// <summary>
        /// Boolean indicating if there are more results in subsequent pages
        /// </summary>
        /// <value>Boolean indicating if there are more results in subsequent pages</value>
        [DataMember(Name="moreResult", EmitDefaultValue=false)]
        public bool MoreResult { get; set; }

        /// <summary>
        /// Paging token given if the result set exceeded the allowed batch size
        /// </summary>
        /// <value>Paging token given if the result set exceeded the allowed batch size</value>
        [DataMember(Name="nextPageToken", EmitDefaultValue=false)]
        public string NextPageToken { get; set; }

        /// <summary>
        /// Id of the request made
        /// </summary>
        /// <value>Id of the request made</value>
        [DataMember(Name="requestId", EmitDefaultValue=false)]
        public string RequestId { get; set; }

        /// <summary>
        /// Array of results for individual records in the operation, may be empty
        /// </summary>
        /// <value>Array of results for individual records in the operation, may be empty</value>
        [DataMember(Name="result", EmitDefaultValue=false)]
        public List<Lead> Result { get; set; }

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
            sb.Append("class ResponseOfLead {\n");
            sb.Append("  Errors: ").Append(Errors).Append("\n");
            sb.Append("  MoreResult: ").Append(MoreResult).Append("\n");
            sb.Append("  NextPageToken: ").Append(NextPageToken).Append("\n");
            sb.Append("  RequestId: ").Append(RequestId).Append("\n");
            sb.Append("  Result: ").Append(Result).Append("\n");
            sb.Append("  Success: ").Append(Success).Append("\n");
            sb.Append("  Warnings: ").Append(Warnings).Append("\n");
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
            return this.Equals(input as ResponseOfLead);
        }

        /// <summary>
        /// Returns true if ResponseOfLead instances are equal
        /// </summary>
        /// <param name="input">Instance of ResponseOfLead to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ResponseOfLead input)
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
                    this.MoreResult == input.MoreResult ||
                    this.MoreResult.Equals(input.MoreResult)
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
                    this.Result == input.Result ||
                    this.Result != null &&
                    input.Result != null &&
                    this.Result.SequenceEqual(input.Result)
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
                hashCode = hashCode * 59 + this.MoreResult.GetHashCode();
                if (this.NextPageToken != null)
                    hashCode = hashCode * 59 + this.NextPageToken.GetHashCode();
                if (this.RequestId != null)
                    hashCode = hashCode * 59 + this.RequestId.GetHashCode();
                if (this.Result != null)
                    hashCode = hashCode * 59 + this.Result.GetHashCode();
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
