using CorePay.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Transactions.MobileApp.Card
{
    public record CardTransferCommand(Guid SenderCardId,
                                      string ReceiverCardNumber,
                                      decimal Amount):IRequest<Result>;
    
}
