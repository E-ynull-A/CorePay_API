using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.GroupProposals.Post.AddMember
{
    public class PostAddMemberProposalCommandValidator:AbstractValidator<PostAddMemberProposalCommand>
    {

        private const string IdentifierRegex = @"^(?:[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}|\+?[1-9]\d{1,14}|0[1-9]\d{8})$";
        public PostAddMemberProposalCommandValidator()
        {

            RuleFor(x => x.TargetUserIdentifier)
            .NotEmpty().WithMessage("Dəvət ediləcək istifadəçinin email və ya telefon nömrəsi daxil edilməlidir.")
            .Matches(IdentifierRegex).WithMessage("Daxil edilən məlumat düzgün Email və ya Telefon nömrəsi formatında deyil.");

        }
    }
}
