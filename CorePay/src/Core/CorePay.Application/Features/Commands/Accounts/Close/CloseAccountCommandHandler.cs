using CorePay.Application.Common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Domain.Entities;
using CorePay.Domain.Utilities.Errors;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Accounts.Close
{
    public class CloseAccountCommandHandler : IRequestHandler<CloseAccountCommand, Result>
    {
        private readonly IAccountRepository _accountRepository;

        public CloseAccountCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public async Task<Result> Handle(CloseAccountCommand request, CancellationToken cancellationToken)
        {
            Account? account = await _accountRepository.GetByIdAsync(request.Id);
            if (account is null)
                return Result.Failure(AccountError.NotFound);

            account.Close();
            _accountRepository.Update(account);

            await _accountRepository.SaveChangesAsync();
            return Result.Success();
        }
    }
}
