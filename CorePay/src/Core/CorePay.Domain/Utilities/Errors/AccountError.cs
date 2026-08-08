using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Utilities.Errors
{
    public sealed class AccountError
    {
        public static Error NotFound { get;} = new Error("Account.NotFound",
                                                  "Account Not Found!",
                                                  ErrorType.NotFound);

        public static Error Dublicate { get;} = new Error("Account.Dublicate",
                                                   "Account was already exist!",
                                                    ErrorType.Dublicate);


    }
}
