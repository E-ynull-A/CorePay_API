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



            RuleFor(t => t.SenderIBAN)
                .NotNull()
                .WithMessage("You cannot send money between the same accounts!")
                .When(t => t.Type == TransactionType.Transfer);



            RuleFor(t => t.CardNumber)
                .NotNull()
                .WithMessage("The Card had to selected for Transfer process!")
                .When(t => t.Type == TransactionType.Transfer);

            RuleFor(t => t.CardNumber)
               .Null()
               .WithMessage("The Card didn't have to sent for Withdraw or Deposit process!")
               .When(t => t.Type == TransactionType.Deposit || t.Type == TransactionType.Withdraw);




            RuleFor(t => t.RecieverIBAN)
                .Null()
                .WithMessage("There is no Reciever IBAN during Withdraw process")
                .When(t => t.Type == TransactionType.Withdraw);

            RuleFor(t => t.RecieverIBAN)
                .NotNull()
                .WithMessage("Receiver IBAN is required for Deposit and Transfer process")
                .When(t => t.Type == TransactionType.Deposit
                        && t.Type == TransactionType.Transfer);




            RuleFor(t => t.SenderIBAN)
                .NotNull()
                .WithMessage("Sender IBAN is required for Transfer and Withdraw process")
                .When(t => t.Type == TransactionType.Transfer 
                        || t.Type == TransactionType.Withdraw);

            RuleFor(t => t.SenderIBAN)
                .Null()
                .WithMessage("Sender IBAN should not be provided for Deposit process")
                .When(t => t.Type == TransactionType.Deposit);


        }
    }

}
