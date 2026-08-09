using CorePay.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Auth.Register
{
    public record RegisterCommand(
        string Name,
        string Surname,
        string Username,
        string Password,
        string Email,
        DateOnly Birthdate,
        string PhoneNumber,
        string FIN):IRequest<Result>;
    
}
