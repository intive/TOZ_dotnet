using System;
using System.ComponentModel.DataAnnotations;

namespace Toz.Dotnet.Resources.CustomValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class PhoneNumberAttribute : ValidationAttribute 
    {    
        public override bool IsValid(object value) 
        {
            if (value == null) 
            {
                return true;
            }
 
            string valueAsString = value as string;

            if (valueAsString == null) 
            {
                return false;
            }

            // remove '+' character and all white spaces
            valueAsString = valueAsString.Replace("+", string.Empty).Replace(" ", string.Empty);

            foreach (char c in valueAsString) 
            {
                if (!Char.IsDigit(c)) 
                {
                    return false;
                }
            }

            if (valueAsString.Length != 9 || valueAsString.Length != 11)
            {
                return false;
            }

            return true;
        }
    }
}