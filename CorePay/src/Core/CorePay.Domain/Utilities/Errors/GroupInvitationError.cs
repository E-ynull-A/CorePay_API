using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Utilities.Errors
{
    public sealed class GroupInvitationError
    {
        public static Error NotFound { get; } = new Error("GroupInvitation.NotFound",
                                                   "Group Invitation is not Found!",
                                                   ErrorType.NotFound);

        public static Error Expired { get; } = new Error("GroupInvitation.Expired",
                                                  "Group Invitation was Expired!",
                                                  ErrorType.BusinessRule);

        public static Error Answered { get; } = new Error("GroupInvitation.Answered",
                                                 "Group Invitation was already answered!",
                                                 ErrorType.BusinessRule);


    }
}
