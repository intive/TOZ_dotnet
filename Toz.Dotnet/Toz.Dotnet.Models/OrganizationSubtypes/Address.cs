using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Toz.Dotnet.Models.OrganizationSubtypes
{
    public class Address
    {
        [JsonProperty("street")]
        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [RegularExpression(@"(([^\u0000-\u007F]|[a-zA-Z])+[\.\-\']?([^\u0000-\u007F]|[a-zA-Z]|(\s(?!$))+)?[\.]?)*", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "InvalidValue")]      
        public string Street { get; set; }

        [JsonProperty("houseNumber")]
        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [RegularExpression("[0-9]{1,3}[a-zA-Z]{0,2}", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "InvalidValue")] 
        public string HouseNumber { get; set; }

        [JsonProperty("apartmentNumber")]
        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [RegularExpression("[0-9]{1,3}", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "InvalidValue")]  
        public string ApartmentNumber { get; set; }

        [JsonProperty("postCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [RegularExpression("[0-9]{2}-[0-9]{3}", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "InvalidValue")]     
        public string PostCode { get; set; }

        [JsonProperty("city")]
        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [RegularExpression(@"(([^\u0000-\u007F]|[a-zA-Z])+[\.\-\']?([^\u0000-\u007F]|[a-zA-Z]|(\s(?!$))+)?[\.]?)*", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "InvalidValue")] 
        public string City { get; set; }

        [JsonProperty("country")]
        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [RegularExpression(@"(([^\u0000-\u007F]|[a-zA-Z])+[\.\-\']?([^\u0000-\u007F]|[a-zA-Z]|(\s(?!$))+)?[\.]?)*", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "InvalidValue")]    
        public string Country { get; set; }
    }
}
