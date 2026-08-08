using CorePay.Domain.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Utilities.Errors.Common
{
    public sealed record Error(string ErrorCode,
                        string Desctription,
                        ErrorType Type);
    
}
