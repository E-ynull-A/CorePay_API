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
        public static Error NotFound { get; } = new Error("Account.NotFound",
                                                  "Account Not Found!",
                                                  ErrorType.NotFound);

        public static Error Dublicate { get; } = new Error("Account.Dublicate",
                                                   "Account was already exist!",
                                                    ErrorType.Conflict);
        public static Error ReachedAccountLimit { get; } = new Error("Account.ReachedLimit",
                                                      "You reach the account limit!",
                                                      ErrorType.BusinessRule);
        public static Error AccountBloked { get; } = new Error("Account.Bloked",
                                                        "The Account was bloked by Bank",
                                                        ErrorType.BusinessRule);

        public static Error AccountClosed { get; } = new Error("Account.Closed",
                                                "The Account was Closed",
                                                ErrorType.BusinessRule);
        public static Error InvalidProperty { get; } = new Error("Account.InvalidProperty",
                                                          "The Property was not found in Account",
                                                          ErrorType.NotFound);


    }
}
