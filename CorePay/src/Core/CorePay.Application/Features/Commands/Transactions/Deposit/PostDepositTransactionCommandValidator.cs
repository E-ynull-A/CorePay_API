using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Transactions.Deposit
{
    public class PostDepositTransactionCommandValidator:AbstractValidator<PostDepositTransactionCommand>
    {
        public PostDepositTransactionCommandValidator()
        {
            RuleFor(dt => dt.CardNumber)
                .NotEmpty()
                .CreditCard().WithMessage("Invalid Card Number Input!");

            RuleFor(dt => dt.Amount)
                .NotEmpty()
                .GreaterThan(0).WithMessage("Invalid Amount Input!");
        }
    }
}
