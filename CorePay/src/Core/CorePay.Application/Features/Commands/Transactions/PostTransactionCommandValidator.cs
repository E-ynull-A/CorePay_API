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

            RuleFor(t => t.PIN)
                .NotEmpty()
                .Must(p=>p.All(d=>char.IsDigit(d)))
                    .WithMessage("PIN consist of only digits!")
                .Must(p => p.Length == 4 || p.Length == 6)
                    .WithMessage("Invalid Password Inout")
                .When(t=>t.Type == TransactionType.Withdraw);



            RuleFor(t => t)
                .Must(t => t.SenderIBAN != t.RecieverIBAN
                       && t.SenderCardNumber != t.RecieverCardNumber)
                .WithMessage("You cannot send money between the same accounts!")
                .When(t => t.Type == TransactionType.Transfer);





            RuleFor(t => t)
                .Must(t => t.RecieverCardNumber is null
                        && t.RecieverIBAN is null)
                .WithMessage("There is no Reciever IBAN or Card Number during Withdraw process")
                .When(t => t.Type == TransactionType.Withdraw);

            RuleFor(t => t)
                .Must(t => (t.RecieverCardNumber is not null
                        && t.RecieverIBAN is null)
                        || (t.RecieverCardNumber is null
                        && t.RecieverIBAN is not null))
                .WithMessage("Receiver IBAN or Reciever Card Number is required for Deposit and Transfer process")
                .When(t => t.Type == TransactionType.Deposit
                        && t.Type == TransactionType.Transfer);




            RuleFor(t => t)
                .Must(t => (t.SenderIBAN is not null
                        && t.SenderCardNumber is null)
                        || (t.SenderIBAN is null
                        && t.SenderCardNumber is not null))
                .WithMessage("Sender IBAN or Card Number is required for Transfer process")
                .When(t => t.Type == TransactionType.Transfer);



            RuleFor(t => t.SenderCardNumber)
                .NotNull()
                .WithMessage("Card Number is required for Withdraw process!")
                .When(t => t.Type == TransactionType.Withdraw);



            RuleFor(t => t)
                .Must(t => t.SenderIBAN is null 
                        && t.SenderCardNumber is null)
                .WithMessage("Sender IBAN and Card Number should not be provided for Deposit process")
                .When(t => t.Type == TransactionType.Deposit);



            RuleFor(t=>t.SenderIBAN)
                .Null()
                .WithMessage("Sender IBAN should not be provided for Withdraw process!")
                .When(t => t.Type == TransactionType.Withdraw);


        }
    }

}
