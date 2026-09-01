using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Utilities.Enums
{
    public enum OtpPurpose
    {
        PasswordReset = 1,
        HighAmountTransfer = 2,
        EmailConfirm = 3,
        Other = 4
    }
}
