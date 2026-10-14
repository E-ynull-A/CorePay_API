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

namespace CorePay.Application.Features.Commands.GroupInvitations.Reject
{
    public class RejectGroupInvitationCommandHandler : IRequestHandler<RejectGroupInvitationCommand, Result>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IGroupInvitationRepository _invitationRepository;

        public RejectGroupInvitationCommandHandler(ICurrentUserService currentUser,
                                                   IGroupInvitationRepository invitationRepository)
        {
            _currentUser = currentUser;
            _invitationRepository = invitationRepository;
        }
        public async Task<Result> Handle(RejectGroupInvitationCommand request, CancellationToken cancellationToken)
        {
            Guid userId = _currentUser.GetUserId();

            GroupInvitation? invitation = await _invitationRepository
                                                .FirstOrDefaultAsync(i => i.Id == request.Id);

            if (invitation == null)
                return Result.Failure(GroupInvitationError.NotFound);

            if (invitation.Status == InvitationStatus.Expired)
                return Result.Failure(GroupInvitationError.Expired);

            invitation.Reject();

            _invitationRepository.Update(invitation);
            await _invitationRepository.SaveChangesAsync();

            return Result.Success();
        }
    }
}
