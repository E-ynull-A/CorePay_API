using CorePay.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Accounts.StatusToggle.User
{
    public record ToggleStatusByUserCommand(Guid AccountId):IRequest<Result>;
    
}
