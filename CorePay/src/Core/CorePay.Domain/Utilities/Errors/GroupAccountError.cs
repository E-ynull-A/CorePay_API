using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Utilities.Errors
{
    public sealed class GroupAccountError
    {
        public static Error NotFound { get; } = new Error("GroupAccount.NotFound",
                                                        "GroupAccount not Found!",
                                                        ErrorType.NotFound);

        public static Error ReachLimit { get; } = new Error("GroupAccount.ReachLimit",
                                                            "Reach the Limit Count of GroupAccount!",
                                                            ErrorType.BusinessRule);
    }
}
