using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Transactions.ATM.Auth
{
    public class AtmAuthenticationCommandValidator:AbstractValidator<AtmAuthenticationCommand>
    {
        public AtmAuthenticationCommandValidator()
        {
            RuleFor(wt => wt.CardNumber)
               .NotEmpty()
               .CreditCard().WithMessage("Invalid Card Number Input!");

           

            RuleFor(wt => wt.PIN)
                .NotEmpty()
                .Must(p => p.All(d => char.IsDigit(d)))
                    .WithMessage("PIN consist of only digits!")
                .Must(p => p.Length == 4 || p.Length == 6)
                    .WithMessage("Invalid Password Inout");
        }
    }
}
