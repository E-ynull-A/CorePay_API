using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Utilities.Errors
{
    public sealed class CardError
    {
        public static Error NotFound { get; } =
            new Error("Card.NotFound", "Card not found!", ErrorType.NotFound);

        public static Error ReachedCardLimit { get; } = new Error("Card.ReachedLimit",
                                                     "You reach the card limit!",
                                                     ErrorType.BusinessRule);

        public static Error Bloked { get; } = new Error("Card.Bloked",
                                                "The Card was bloked by Bank",
                                                ErrorType.BusinessRule);

        public static Error Expired { get; } = new Error("Card.Expired",
                                                 "The Card was Expired!" +
                                                 "You need to get a new one",
                                                 ErrorType.BusinessRule);
    }
}
