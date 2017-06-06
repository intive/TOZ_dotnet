using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Toz.Dotnet.Models.EnumTypes;
using Toz.Dotnet.Models.JsonConventers;
using Toz.Dotnet.Models.Validation;

namespace Toz.Dotnet.Models
{
    public class Helper
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        [StringLength(35, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]
        public string Name { get; set; }

        [JsonProperty("surname")]
        [StringLength(35, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]
        public string Surname { get; set; }

        [JsonProperty("notes")]
        [StringLength(500, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]
        public string Notes { get; set; }

        [JsonProperty("phoneNumber")]
        [PhoneNumber(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "InvalidPhoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("email")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmailValidationMessage")]
        public string Email { get; set; }

        [JsonProperty("address")]
        [StringLength(255, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]
        public string Address { get; set; }

        [JsonProperty("category")]
        [JsonConverter(typeof(StringEnumConverter))]
        [Range(1, 4, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "HelperCategoryUndefined")]
        public HelperCategory Category { get; set; }

        [JsonProperty("created")]
        [JsonConverter(typeof(JsonDateTimeConventer))]
        public DateTime Created { get; set; }

        [JsonProperty("lastModified")]
        [JsonConverter(typeof(JsonDateTimeConventer))]
        public DateTime LastModified { get; set; }

        public string DisplayedName
        {
            get
            {
                if (string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Surname))
                {
                    return (string.IsNullOrEmpty(Address)) ? "" : Address;
                }
                else
                {
                    string fullName = (Name + " " + Surname).Trim();
                    return (string.IsNullOrEmpty(Address)) ? fullName : fullName + ", " + Address;
                }            
            }
        }
    }
}
