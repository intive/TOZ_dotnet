using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace Toz.Dotnet.Models.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class BankAccountNumberAttribute : ValidationAttribute
    {
        private const string PolandIbanPrefix = "PL";

        public override bool IsValid(object value)
        {
            var bankAccount = value as string;
            if (string.IsNullOrEmpty(bankAccount))
            {
                return true;
            }

            // Remove all white spaces
            bankAccount = bankAccount.Replace(" ", string.Empty);

            switch (bankAccount.Length)
            {
                // Validate length
                case 26:
                    bankAccount = $"{PolandIbanPrefix}{bankAccount}";
                    break;
                default:
                    // Enforce Polish country prefix
                    if (Regex.IsMatch(bankAccount, @"[\D]{2}[0-9]{26}") && bankAccount.StartsWith("PL", StringComparison.CurrentCultureIgnoreCase))
                    {
                        bankAccount = $"{PolandIbanPrefix}{bankAccount.Substring(2, 26)}";
                    }
                    else
                    {
                        return false;
                    }
                    break;
            }

            if (Regex.IsMatch(bankAccount, "^[A-Z0-9]"))
            {
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
