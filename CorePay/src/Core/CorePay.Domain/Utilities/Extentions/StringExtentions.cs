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
    }
}
