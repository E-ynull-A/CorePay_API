using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Transactions.MobileApp.CardToCard
{
    public class CardTransferCommandValidator:AbstractValidator<CardTransferCommand>
    {
        public CardTransferCommandValidator()
        {
            RuleFor(c => c.SenderCardId)
                .NotEmpty();

            RuleFor(c=>c.ReceiverCardNumber)
                .NotEmpty()
                .CreditCard()
                .WithMessage("Invalid Card Number Input!");

            RuleFor(t => t.Amount)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("The Amount of Money must be greater than zero!");


        }
    }
}
