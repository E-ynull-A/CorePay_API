using CorePay.Domain.Utilities.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.GroupAccounts.Post
{
    public class PostGroupAccountCommandValidator:AbstractValidator<PostGroupAccountCommand>
    {
        public PostGroupAccountCommandValidator()
        {
            RuleFor(ga => ga.Currency)
                .NotEmpty()
                .IsInEnum()
                .WithMessage("Invalid Currency Input!");

            RuleFor(ga => ga.Title)
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(ga => ga.Description)
                .MaximumLength(1028);
                
        }
    }
}
