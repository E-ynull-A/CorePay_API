using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Utilities.Errors
{
    public sealed class TransactionError
    {
        public static Error NoEnoughBalance { get; } = new Error("Transaction.NotEnoughBalance",
                                                                 "There no enough balance for the Transaction in your Account!",
                                                                 ErrorType.BusinessRule);
        public static Error InvalidStatus { get; } = new Error("Transaction.InvalidStatus",
                                                               "Transaction process was required an active Account and Card!",
                                                                ErrorType.BusinessRule);
        public static Error WrongPIN { get; } = new Error("Transaction.WrongPIN",
                                                          "PIN Code of the Card is wrong!",
                                                          ErrorType.BusinessRule);

        public static Error TooManyAttempts { get; } = new Error("Transaction.TooManyAttempts",
                                                                 "Too many Attempts! Please, try again Later!",
                                                                 ErrorType.BusinessRule);
                    }
}
