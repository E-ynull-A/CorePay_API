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

namespace CorePay.Application.Features.Commands.Accounts.Close
{
    public class CloseAccountCommandHandler : IRequestHandler<CloseAccountCommand, Result>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRedisCasheService _casheService;

        public CloseAccountCommandHandler(IAccountRepository accountRepository,
                                          IRedisCasheService casheService)
        {
            _accountRepository = accountRepository;
            _casheService = casheService;
        }
        public async Task<Result> Handle(CloseAccountCommand request, CancellationToken cancellationToken)
        {
            Account? account = await _accountRepository.GetByIdAsync(request.Id);
            if (account is null)
                return Result.Failure(AccountError.NotFound);

            string otpPurpose = OtpPurpose.CloseAccount.ToString().ToLower();
            string key = $"otp-confirmed:{otpPurpose}:{account.AppUserId}";

            if (!await _casheService.AnyAsync(key))
                return Result.Failure(AuthError.FailedOtpConfirmation);

            await _casheService.DeleteAsync(key);

            account.Close();
            _accountRepository.Update(account);

            await _accountRepository.SaveChangesAsync();
            return Result.Success();
        }
    }
}
