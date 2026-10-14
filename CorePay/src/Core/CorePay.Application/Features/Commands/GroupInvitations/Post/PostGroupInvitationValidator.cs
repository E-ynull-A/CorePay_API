using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.GroupInvitations.Post
{
    public class PostGroupInvitationValidator:AbstractValidator<PostGroupInvitationCommand>
    {
        public PostGroupInvitationValidator()
        {
            RuleFor(gi => gi.GroupAccountId)
                .NotEmpty();
            RuleFor(gi=>gi.InvitedUserId)
                .NotEmpty();
        }
    }
}
