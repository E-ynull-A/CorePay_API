using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Utilities.Enums
{
    public enum CriticalOtpPurpose
    {
        PasswordReset = 1,
        HighAmountTransfer = 2,
        CloseAccount = 3,
        DeleteCard = 4
    }
}
