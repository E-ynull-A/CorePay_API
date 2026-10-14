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

namespace CorePay.Application.Features.Commands.GroupProposals.Post
{
    public class PostGroupProposalCommandHandler : IRequestHandler<PostGroupProposalCommand,Result>
    {
        private readonly IGroupProposalRepository _proposalRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IGroupAccountRepository _groupAccountRepository;

        public PostGroupProposalCommandHandler(IGroupProposalRepository proposalRepository,
                                               ICurrentUserService currentUser,
                                               IGroupAccountRepository groupAccountRepository)
        {
            _proposalRepository = proposalRepository;
            _currentUser = currentUser;
            _groupAccountRepository = groupAccountRepository;
        }

        public async Task<Result> Handle(PostGroupProposalCommand request, CancellationToken cancellationToken)
        {
            Guid userId = _currentUser.GetUserId();

            GroupAccount? gAccount = await _groupAccountRepository
                                            .FirstOrDefaultAsync(ga => ga.GroupMembers.Any
                                                                (gm => gm.Id == request.GroupMemberId),
                                                                [nameof(GroupAccount.GroupMembers)]);

            if (gAccount == null)
                return Result.Failure(GroupAccountError.NotFound);

          
            if (gAccount.Status != GroupAccountStatus.Active)
                return Result.Failure(GroupAccountError.InactiveStatus);

            if (gAccount.GroupMembers
                .First(gm => gm.Id == request.GroupMemberId).Status
                != GroupMemberStatus.Active)
                     return Result.Failure(GroupAccountError.InactiveMember);

            GroupProposal groupProposal = new GroupProposal(gAccount.Id,request.GroupMemberId,
                                                            request.Type,request.Amount);

            _proposalRepository.Add(groupProposal); 
            gAccount.Proposals.Add(groupProposal); 

            _groupAccountRepository.Update(gAccount);

            await _proposalRepository.SaveChangesAsync();
            return Result.Success();            
        }
    }
}
