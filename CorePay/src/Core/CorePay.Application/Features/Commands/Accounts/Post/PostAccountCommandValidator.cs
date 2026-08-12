using CorePay.Domain.Utilities.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Accounts.Post
{
    public class PostAccountCommandValidator:AbstractValidator<PostAccountCommand>
    {
        public PostAccountCommandValidator()
        {
            RuleFor(a => a.Currency)
                .IsInEnum();
        }
    }
}
