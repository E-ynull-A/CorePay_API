using CorePay.Application.Common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Entities;
using CorePay.Domain.Utilities.Errors;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.GroupProposals.Post.RemoveMember
{
    public class PostRemoveMemberProposalCommandHandler : IRequestHandler<PostRemoveMemberProposalCommand, Result>
    {
        private readonly IGroupAccountRepository _groupAccountRepository;
        private readonly ITransferRepository _transferRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrentUserService _userService;

        public PostRemoveMemberProposalCommandHandler(IGroupAccountRepository groupAccountRepository,
                                                      ITransferRepository transferRepository,
                                                      ITransactionRepository transactionRepository,
                                                      ICurrentUserService userService)
        {
            _groupAccountRepository = groupAccountRepository;
            _transferRepository = transferRepository;
            _transactionRepository = transactionRepository;
            _userService = userService;
        }
        public async Task<Result> Handle(PostRemoveMemberProposalCommand request, CancellationToken cancellationToken)
        {
            Guid userId = _userService.GetUserId();

            GroupAccount? gAccount = await _groupAccountRepository
                                                .FirstOrDefaultAsync(ga=>ga.GroupMembers
                                                             .Any(gm=>gm.AppUserId == userId),
                                                             [nameof(GroupAccount.GroupMembers),
                                                              nameof(GroupAccount.Account)]);

            if (gAccount is null)
                return Result.Failure(GroupAccountError.NotFound);

            
            


            decimal income = await _transactionRepository.SumAsync(t => t.Amount,
                                                  t => t.Transfer.RecieverAccountId 
                                                            == gAccount.AccountId
                                                    && t.Transfer.SenderAccount.AppUserId
                                                            == userId);

            decimal outcome = await _transactionRepository.

               //özü çıxardan pul məsələsi


        }
    }
}
