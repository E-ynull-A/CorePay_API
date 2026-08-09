using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Auth.Login
{
    public class LoginCommandValidation:AbstractValidator<LoginCommand>
    {
        public LoginCommandValidation()
        {
            RuleFor(l => l.UsernameOrEmail)
                .NotEmpty()
                .MaximumLength(256)
                .MinimumLength(3)
                .WithMessage("Username or Email is Invalid!");

            RuleFor(l => l.Password)
                .NotEmpty()
                .MinimumLength(7)
                .MaximumLength(256)
                .Must(l => l.Any(l => char.IsUpper(l))
                        && l.Any(l => char.IsLower(l))
                        && l.Any(l => char.IsDigit(l)));
        }
    }
}
