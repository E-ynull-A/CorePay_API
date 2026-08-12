using CorePay.Application.Common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Domain.Entities;
using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Accounts.StatusToggle.User
{
    public class ToggleStatusByUserCommandHandler : IRequestHandler<ToggleStatusByUserCommand, Result>
    {
        private readonly IAccountRepository _accountRepository;

        public ToggleStatusByUserCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public async Task<Result> Handle(ToggleStatusByUserCommand request, CancellationToken cancellationToken)
        {
            Account? account = await _accountRepository
                .FirstOrDefaultAsync(a => a.Id == request.AccountId);


            switch (account.Status)
            {
                case AccountStatus.Active:
                    account.BlokedByUser();
                    break;
                case AccountStatus.UserBlocked:
                    account.Activate();
                    break;
                case AccountStatus.BankBloked:
                    return Result.Failure(AccountError.AccountBloked);
                case AccountStatus.Closed:
                    return Result.Failure(AccountError.AccountClosed);
            }

             _accountRepository.Update(account);
            await _accountRepository.SaveChangesAsync();

            return Result.Success();    
        }
    }
}
