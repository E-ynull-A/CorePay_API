using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Utilities.Extentions
{
    public static class TransferRequestHasher
    {

        public static string Create(Guid senderAccountId,
                                    string receiverIban,
                                    decimal amount)
        {
            string normalizedIban = new string(
                receiverIban
                        .Where(c => !char.IsWhiteSpace(c))
                        .ToArray())
                .ToUpperInvariant();


            string requestData = $"{senderAccountId:D}|{normalizedIban}|{amount.ToString(CultureInfo.InvariantCulture)}";
                    
              byte[] bytes =  Encoding.UTF8.GetBytes(requestData);
            var hash = SHA256.HashData(bytes);


            return Convert.ToHexString(hash);

        }
    }
}
