using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
using Microsoft.Extensions.Localization;

namespace Toz.Dotnet.Models.Validation
{
    public class CustomValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
    {
        private readonly IValidationAttributeAdapterProvider baseProvider = new ValidationAttributeAdapterProvider();

        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            return (attribute is BankAccountNumberAttribute)
                ? new BankAccountNumberAttributeAdapter(attribute as BankAccountNumberAttribute, stringLocalizer)
                : baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
        }
    }
}
