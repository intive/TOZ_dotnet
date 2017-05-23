﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Toz.Dotnet.Models
{
    public class UserBase : Login
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [JsonProperty("name")]
        [StringLength(35, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [JsonProperty("surname")]
        [StringLength(35, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]
        public string LastName { get; set; }

        public string CombinedName => string.Format("{0} {1}",
            string.IsNullOrEmpty(LastName) ? string.Empty : LastName + ", ",
            string.IsNullOrEmpty(FirstName) ? string.Empty : FirstName);

        /// <summary>
        /// Returns a human-readable user description
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0} {1}",
                string.IsNullOrEmpty(LastName) ? string.Empty : LastName + ", ",
                string.IsNullOrEmpty(FirstName) ? string.Empty : FirstName);
        }
    }
}
