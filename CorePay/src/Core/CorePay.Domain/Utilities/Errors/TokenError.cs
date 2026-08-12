using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Utilities.Errors
{
    public sealed class TokenError
    {
        public static Error NotFound { get; } =
            new Error("Token.NotFound", "Token not Found!", ErrorType.NotFound);

        public static Error Revoked { get; } =
            new Error("Token.Revoked", "Token was Revoked!", ErrorType.Unauthorized);

        public static Error Expired { get; } =
            new Error("Token.Expired","Token was Expired",ErrorType.Unauthorized);
    }
}
