using CorePay.Domain.Utilities.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CorePay.Application.Features.Commands.Transactions
{
    public class PostTransactionCommandValidator : AbstractValidator<PostTransactionCommand>
    {
        public PostTransactionCommandValidator()
        {
            RuleFor(t => t.Amount)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("The Amount of Money must be greater than zero!");

            RuleFor(t => t.Type)
                .NotEmpty()
                .IsInEnum();



            RuleFor(t => t)
                .Must(t => t.senderAccount != t.recieverAccount
                       && t.SenderCardNumber != t.RecieverCardNumber)
                .WithMessage("You cannot send money between the same accounts!")
                .When(t => t.Type == TransactionType.Transfer);





            RuleFor(t => t)
                .Must(t => t.RecieverCardNumber is null
                        && t.recieverAccount is null)
                .WithMessage("There is no Reciever IBAN or Card Number during Withdraw process")
                .When(t => t.Type == TransactionType.Withdraw);

            RuleFor(t => t)
                .Must(t => (t.RecieverCardNumber is not null
                        && t.recieverAccount is null)
                        || (t.RecieverCardNumber is null
                        && t.recieverAccount is not null))
                .WithMessage("Receiver IBAN or Reciever Card Number is required for Deposit and Transfer process")
                .When(t => t.Type == TransactionType.Deposit
                        && t.Type == TransactionType.Transfer);




            RuleFor(t => t)
                .Must(t => (t.senderAccount is not null
                        && t.SenderCardNumber is null)
                        || (t.senderAccount is null
                        && t.SenderCardNumber is not null))
                .WithMessage("Sender IBAN or Card Number is required for Transfer and Withdraw process")
                .When(t => t.Type == TransactionType.Transfer
                        || t.Type == TransactionType.Withdraw);

            RuleFor(t => t)
                .Must(t => t.senderAccount is null 
                        && t.SenderCardNumber is null)
                .WithMessage("Sender IBAN and Card Number should not be provided for Deposit process")
                .When(t => t.Type == TransactionType.Deposit);


        }
    }

}
