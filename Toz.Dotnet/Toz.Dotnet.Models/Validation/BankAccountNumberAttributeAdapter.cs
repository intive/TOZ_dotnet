using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;

namespace Toz.Dotnet.Models.Validation
{
    public class BankAccountNumberAttributeAdapter : AttributeAdapterBase<BankAccountNumberAttribute>
    {
        public BankAccountNumberAttributeAdapter(BankAccountNumberAttribute attribute,
            IStringLocalizer stringLocalizer)
            : base(attribute, stringLocalizer)
        {
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-accountnumber", GetErrorMessage(context));
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            return GetErrorMessage(validationContext.ModelMetadata, 
                validationContext.ModelMetadata.GetDisplayName());
        }
    }
}
