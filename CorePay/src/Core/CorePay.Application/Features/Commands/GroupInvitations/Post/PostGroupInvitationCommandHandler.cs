using CorePay.Application.Common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Domain.Entities;
using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.GroupInvitations.Post
{
    public class PostGroupInvitationCommandHandler : IRequestHandler<PostGroupInvitationCommand, Result>
    {
        private readonly IGroupInvitationRepository _groupInvitationRepository;
        private readonly IGroupAccountRepository _accountRepository;
        private readonly UserManager<AppUser> _userManager;

        public PostGroupInvitationCommandHandler(IGroupInvitationRepository groupInvitationRepository,
                                                 IGroupAccountRepository accountRepository,
                                                 UserManager<AppUser> userManager)
        {
            _groupInvitationRepository = groupInvitationRepository;
            _accountRepository = accountRepository;
            _userManager = userManager;
        }

        public async Task<Result> Handle(PostGroupInvitationCommand request, CancellationToken cancellationToken)
        {
            if (!await _userManager.Users.AnyAsync(u => u.Id == request.InvitedUserId))
                return Result.Failure(AuthError.NotFound);

            GroupAccount? account = await _accountRepository
                                        .FirstOrDefaultAsync(ga => ga.Id == request.GroupAccountId,
                                                            [nameof(GroupAccount.GroupMembers)]);

            if(account is null)
                return Result.Failure(GroupAccountError.NotFound);

            if (account.Status != GroupAccountStatus.Active)
                return Result.Failure(GroupAccountError.InactiveStatus);

            if (account.GroupMembers.Any(gm => gm.AppUserId == request.InvitedUserId))
                return Result.Failure(GroupAccountError.SelfInvite);
                
            GroupInvitation groupInvitation = new GroupInvitation(request.GroupAccountId,
                                                                  request.InvitedUserId);

            _groupInvitationRepository.Add(groupInvitation);
            await _groupInvitationRepository.SaveChangesAsync();

            return Result.Success();
        }
    }
}
