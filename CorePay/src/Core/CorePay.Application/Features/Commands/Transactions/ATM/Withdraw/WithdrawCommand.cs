using CorePay.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Transactions.ATM.Withdraw
{
    public record WithdrawCommand(string SessionId,
                                  decimal Amount):IRequest<Result>;
   
}
