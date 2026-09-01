using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Auth.EmailConfirm.Send
{
    public class EmailConfirmCommandValidator:AbstractValidator<EmailConfirmCommand>
    {
        public EmailConfirmCommandValidator()
        {
            RuleFor(ec => ec.Code)
               .NotEmpty()
               .Must(ec => ec.All(l => char.IsDigit(l)))
                   .WithMessage("Invalid Code Input!");
        }
    }
}
