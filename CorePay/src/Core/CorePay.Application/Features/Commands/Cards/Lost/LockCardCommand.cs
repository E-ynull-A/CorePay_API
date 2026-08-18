using CorePay.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Cards.Lost
{
    public record LockCardCommand(Guid Id):IRequest<Result>;
   
}
