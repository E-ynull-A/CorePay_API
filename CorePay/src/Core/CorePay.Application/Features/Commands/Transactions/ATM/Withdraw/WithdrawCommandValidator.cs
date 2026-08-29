using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Transactions.ATM.Withdraw
{
    public class WithdrawCommandValidator:AbstractValidator<WithdrawCommand>
    {
        public WithdrawCommandValidator()
        {
            RuleFor(w => w.Amount)
               .NotEmpty()
               .GreaterThan(0).WithMessage("Invalid Amount Input!");

            RuleFor(w => w.SessionId)
                .NotEmpty()
                .MaximumLength(40);
        }
    }
}
