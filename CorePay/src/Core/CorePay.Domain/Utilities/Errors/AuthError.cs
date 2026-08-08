using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Utilities.Errors
{
    public sealed class AuthError
    {
        public static Error NotFound { get; } = new Error("User.NotFound",
                                                  "User Not Found!",
                                                  ErrorType.NotFound);

        public static Error Dublicate { get; } = new Error("User.Dublicate",
                                                   "User was already exist!",
                                                    ErrorType.Dublicate);

        public static Error Lockout { get; } = new Error("User.Lockout",
                                                   "User's Lockout is enable!",
                                                    ErrorType.NotAllowed);
    }
}
