using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Utilities.Enums
{
    public enum ErrorType
    {
        None = 0,
        NotFound = 1,
        Dublicate = 2,
        Validation = 3,
        NotAllowed = 4
    }
}
