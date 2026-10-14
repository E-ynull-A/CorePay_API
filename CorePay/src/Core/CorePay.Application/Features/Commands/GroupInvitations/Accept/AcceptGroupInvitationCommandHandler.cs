using CorePay.Application.Common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Entities;
using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.GroupInvitations.Accept
{
    public class AcceptGroupInvitationCommandHandler : IRequestHandler<AcceptGroupInvitationCommand, Result>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IGroupInvitationRepository _invitationRepository;
        private readonly IGroupAccountRepository _groupAccountRepository;

        public AcceptGroupInvitationCommandHandler(ICurrentUserService currentUser,
                                                   IGroupInvitationRepository invitationRepository,
                                                   IGroupAccountRepository groupAccountRepository)
        {
            _currentUser = currentUser;
            _invitationRepository = invitationRepository;
            _groupAccountRepository = groupAccountRepository;
        }
        public async Task<Result> Handle(AcceptGroupInvitationCommand request, CancellationToken cancellationToken)
        {
            Guid userId = _currentUser.GetUserId();

            GroupInvitation? invitation = await _invitationRepository
                                                .FirstOrDefaultAsync(i => i.Id == request.Id);

            if (invitation == null)
                return Result.Failure(GroupInvitationError.NotFound);

            if (invitation.Status == InvitationStatus.Expired)
                return Result.Failure(GroupInvitationError.Expired);

            if (invitation.Status != InvitationStatus.Pending)
                return Result.Failure(GroupInvitationError.Answered);

            invitation.Accept();

            GroupAccount? gAcoount = await _groupAccountRepository
                                            .FirstOrDefaultAsync(i => i.Id == invitation.GroupAccounId);

            if (gAcoount == null)
                return Result.Failure(GroupAccountError.NotFound);

            GroupMember groupMember = new GroupMember(userId,gAcoount);

            gAcoount.GroupMembers.Add(groupMember);
            _groupAccountRepository.Update(gAcoount);

            _invitationRepository.Update(invitation);
            await _invitationRepository.SaveChangesAsync();
            
            return Result.Success();
        }
    }
}
