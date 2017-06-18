using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Toz.Dotnet.Models
{
    public class ForgotPassword
    {
        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [JsonProperty("email")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmailValidationMessage")]
        public string Email { get; set; }
    }
}
