using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Utilities.Extentions
{
    public static class StringExtentions
    {
        public static string Capitalize(this string word)
        {
            word = word.Trim();

            string[] words = word.Split(" ");
            string result = string.Empty;

            foreach (var wrd in words)
            {
                result += char.ToUpperInvariant(wrd[0]) + wrd[1..];
            }

            return result;
        }

        public static string MaskIban(this string iban) =>
           $"{iban[..4]} {iban[4..8]} ** **** {iban[^4..]}";


        public static string MaskCardNumber(this string cardNumber) =>
            $"{cardNumber[..4]} {cardNumber[4..6]} ** {cardNumber[^4..]}";
      
    }
}
