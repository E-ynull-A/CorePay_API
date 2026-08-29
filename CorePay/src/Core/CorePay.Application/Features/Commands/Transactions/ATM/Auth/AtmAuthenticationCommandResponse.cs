using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Transactions.ATM.Auth
{
    public record AtmAuthenticationCommandResponse(string sessionId);
  
}
