using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Utilities.Enums
{
    public enum OtpPurpose
    {
        EmailConfirm = 1,
        PasswordReset = 2,
        HighAmountTransfer = 3
    }
}
