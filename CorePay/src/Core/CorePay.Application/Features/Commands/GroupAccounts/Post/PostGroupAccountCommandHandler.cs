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

namespace CorePay.Application.Features.Commands.GroupAccounts.Post
{
    public class PostGroupAccountCommandHandler : IRequestHandler<PostGroupAccountCommand, Result>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IGroupAccountRepository _groupAccountRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISystemValueGeneratorService _valueGeneratorService;

        public PostGroupAccountCommandHandler(IAccountRepository accountRepository,
                                              IGroupAccountRepository groupAccountRepository,
                                              ICurrentUserService currentUserService,
                                              ISystemValueGeneratorService valueGeneratorService)
        {
            _accountRepository = accountRepository;
            _groupAccountRepository = groupAccountRepository;
            _currentUserService = currentUserService;
            _valueGeneratorService = valueGeneratorService;
        }

        public async Task<Result> Handle(PostGroupAccountCommand request, CancellationToken cancellationToken)
        {

            Guid userId = _currentUserService.GetUserId();

            if (await _groupAccountRepository.CountAsync(gr => gr.Account.AppUserId == userId) == 5)
                return Result.Failure(GroupAccountError.ReachLimit);


            string iban = await _valueGeneratorService.GenerateIbanAsync();

            Account account = new Account(iban,request.Currency,
                                            AccountType.Group);

            _accountRepository.Add(account);

            GroupAccount groupAccount = new GroupAccount(request.Title,account,
                                                        DateTimeOffset.Now.AddDays(request.ExpireDays));

            GroupMember groupCreator = new GroupMember(userId,groupAccount);

            groupAccount.GroupMembers.Add(groupCreator);
            _groupAccountRepository.Add(groupAccount);

            await _groupAccountRepository.SaveChangesAsync();
            return Result.Success();
        }
    }
}
