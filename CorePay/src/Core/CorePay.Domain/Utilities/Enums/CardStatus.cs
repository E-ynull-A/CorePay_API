using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Utilities.Enums
{
    public enum CardStatus
    {
        Active = 1,
        Blocked = 2,
        Expired = 3,
        Lost = 4
    }
}
