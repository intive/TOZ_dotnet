using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Toz.Dotnet.Resources;

namespace Toz.Dotnet.Models.CustomValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class BankAccountNumberAttribute : ClientSideValidationAttributeBase
    {
        private const string PolandIbanPrefix = "PL";

        public override bool IsValid(object value)
        {
            var bankAccount = value as string;
            if (bankAccount == null)
            {
                return false;
            }
            bankAccount = $"{PolandIbanPrefix}{bankAccount}";
            bankAccount = bankAccount.ToUpper();
            if (string.IsNullOrEmpty(bankAccount))
            {
                return false;
            }
            if (System.Text.RegularExpressions.Regex.IsMatch(bankAccount, "^[A-Z0-9]"))
            {
                bankAccount = bankAccount.Replace(" ", string.Empty);
                string bank = bankAccount.Substring(4, bankAccount.Length - 4) + bankAccount.Substring(0, 4);
                int asciiShift = 55;
                var stringBuilder = new StringBuilder();
                foreach (char c in bank)
                {
                    int v;
                    if (char.IsLetter(c))
                    {
                        v = c - asciiShift;
                    }
                    else
                    {
                        v = int.Parse(c.ToString());
                    }
                    stringBuilder.Append(v);
                }
                string checkSumString = stringBuilder.ToString();
                int checksum = int.Parse(checkSumString.Substring(0, 1));
                for (int i = 1; i < checkSumString.Length; i++)
                {
                    int v = int.Parse(checkSumString.Substring(i, 1));
                    checksum *= 10;
                    checksum += v;
                    checksum %= 97;
                }
                return checksum == 1;
            }
            return false;
        }

        
    }
}
