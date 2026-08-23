using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Cards.Post
{
    public class PostCardCommandValidator:AbstractValidator<PostCardCommand>
    {
        public PostCardCommandValidator()
        {
            RuleFor(c => c.PIN)
                .NotEmpty()
                .Must(p => p.Length == 4 || p.Length == 6)
                .WithMessage("PIN length must be 4 or 6!");
        }
    }
}
