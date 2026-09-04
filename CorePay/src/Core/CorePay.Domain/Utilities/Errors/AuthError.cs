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
        public static Error NotFound { get; } = new("User.NotFound",
                                                  "User Not Found!",
                                                  ErrorType.NotFound);

        public static Error Dublicate { get; } = new("User.Dublicate",
                                                   "User was already exist!",
                                                    ErrorType.Conflict);

        public static readonly Error AccountLockedOut = new("Auth.AccountLockedOut",
                                                            "Your account is temporarily locked.",
                                                            ErrorType.Forbidden);

        public static readonly Error InvalidCredentials = new("Auth.InvalidCredentials",
                                                              "Username or password is incorrect.",
                                                              ErrorType.Unauthorized);

        public static readonly Error TooManyRequests = new("Auth.ManyRequests",
                                                              "Too many requests! " +
                                                              "Please try a few minutes later.",
                                                              ErrorType.TooManyRequests);
        public static readonly Error FailedOtpConfirmation = new("Auth.OtpConfirmation",
                                                                "Otp Code not Found or has already Expired",
                                                                ErrorType.Validation);

    }
}
