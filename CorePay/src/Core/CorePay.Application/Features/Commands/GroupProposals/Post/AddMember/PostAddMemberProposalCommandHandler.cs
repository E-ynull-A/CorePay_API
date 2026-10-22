using CorePay.Application.Common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Application.Interfaces.Services;
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

namespace CorePay.Application.Features.Commands.GroupProposals.Post.AddMember
{
    public class PostAddMemberProposalCommandHandler : IRequestHandler<PostAddMemberProposalCommand, Result>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IGroupAccountRepository _groupAccountRepository;
        private readonly IGroupProposalRepository _proposalRepository;
        private readonly UserManager<AppUser> _userManager;

        public PostAddMemberProposalCommandHandler(ICurrentUserService currentUser,
                                                   IGroupAccountRepository groupAccountRepository,
                                                   IGroupProposalRepository proposalRepository,
                                                   UserManager<AppUser> userManager)
        {
            _currentUser = currentUser;
            _groupAccountRepository = groupAccountRepository;
            _proposalRepository = proposalRepository;
            _userManager = userManager;
        }
        public async Task<Result> Handle(PostAddMemberProposalCommand request, CancellationToken cancellationToken)
        {
            Guid userId = _currentUser.GetUserId();

            GroupAccount? gAccount = await _groupAccountRepository
                                            .FirstOrDefaultAsync(ga => ga.GroupMembers.Any
                                                                (gm => gm.AppUserId == userId),
                                                                [nameof(GroupAccount.GroupMembers)]);

            GroupMember purposeCreator = gAccount.GroupMembers.First(gm => gm.AppUserId == userId);



            if (gAccount == null)
                return Result.Failure(GroupAccountError.NotFound);


            if (gAccount.Status != GroupAccountStatus.Active)
                return Result.Failure(GroupAccountError.InactiveStatus);

            if (purposeCreator.Status != GroupMemberStatus.Active)
                return Result.Failure(GroupAccountError.InactiveMember);

            AppUser? invitedUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == request.TargetUserIdentifier
                                                                               || u.Email == request.TargetUserIdentifier);

            if (invitedUser is null)
                return Result.Failure(AuthError.NotFound);

            if (await _groupAccountRepository.AnyAsync(ga => ga.GroupMembers.Any(gm => gm.AppUser.Email == request.TargetUserIdentifier
                                                                          || gm.AppUser.UserName == request.TargetUserIdentifier)))
                return Result.Failure(GroupAccountError.SelfMemberInvite);

            GroupProposal groupProposal = new GroupProposal(gAccount.Id, purposeCreator.Id,
                                                            ProposalType.AddMember, targetUserId: invitedUser.Id);

            _proposalRepository.Add(groupProposal);
            gAccount.Proposals.Add(groupProposal);

            _groupAccountRepository.Update(gAccount);
            await _proposalRepository.SaveChangesAsync();

            return Result.Success();
        }
    }
}
