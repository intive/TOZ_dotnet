using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Toz.Dotnet.Models.CustomValidationAttributes
{
    public abstract class ClientSideValidationAttributeBase : ValidationAttribute, IClientModelValidator
    {
        private const string PartToDelete = "Attribute";
        public void AddValidation(ClientModelValidationContext context)
        {
            var className = this.GetType().Name;
            if (className.EndsWith(PartToDelete))
            {
                className = className.Remove(className.Length - PartToDelete.Length, PartToDelete.Length).ToLower();
            }
            MergeAttribute(context.Attributes, "data-val", "true");
            var errorMessage = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            MergeAttribute(context.Attributes, $"data-val-{className}", errorMessage);
        }

        private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }
            attributes.Add(key, value);
            return true;
        }
    }
}
