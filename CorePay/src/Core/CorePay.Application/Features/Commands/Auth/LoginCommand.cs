using CorePay.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Auth
{
    public record LoginCommand(       
        string UsernameOrEmail,
        string Password):IRequest<Result<LoginCommandResponce>>;
   
}
