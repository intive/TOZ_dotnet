using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Toz.Dotnet.Models.OrganizationSubtypes
{
    public class Address
    {
        [JsonProperty("street")]
        [RegularExpression(@"[\p{L}-]+", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "InvalidValue")]
        //[Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        public string Street { get; set; }

        [JsonProperty("houseNumber")]
        [RegularExpression("[0-9]{1-3}[a-zA-Z]{1}", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "InvalidValue")]
        //[Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        public string HouseNumber { get; set; }

        [JsonProperty("apartmentNumber")]
        [RegularExpression("[0-9]{1-3}", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "OnlyDigits")]
        //[Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        public string ApartmentNumber { get; set; }

        [JsonProperty("postCode")]
        [RegularExpression("[0-9]{2}-[0-9]{3}", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "InvalidValue")]
        //[Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        public string PostCode { get; set; }

        [JsonProperty("city")]
        [RegularExpression(@"[\p{L}-]+", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "InvalidValue")]
        //[Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        public string City { get; set; }

        [JsonProperty("country")]
        [RegularExpression(@"[\p{L}-]+", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "InvalidValue")]
        //[Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        public string Country { get; set; }
    }
}
