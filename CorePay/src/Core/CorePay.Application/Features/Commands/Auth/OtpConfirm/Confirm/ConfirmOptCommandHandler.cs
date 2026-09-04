using CorePay.Application.Common;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors;
using CorePay.Domain.Utilities.Errors.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Auth.OtpConfirm.Confirm
{
    public class ConfirmOptCommandHandler : IRequestHandler<ConfirmOptCommand, Result>
    {
        private readonly IRedisCasheService _casheService;
        private readonly ICurrentUserService _currentUser;
        private readonly IOtpService _otpService;

        private const int EXP_MINUTE = 3;

        public ConfirmOptCommandHandler(IRedisCasheService casheService,
                                        ICurrentUserService currentUser,
                                        IOtpService otpService)
        {
            _casheService = casheService;
            _currentUser = currentUser;
            _otpService = otpService;
        }
        public async Task<Result> Handle(ConfirmOptCommand request, CancellationToken cancellationToken)
        {
            string userEmail = _currentUser.GetUserEmail();

            OtpPurpose purpose = request.Purpose switch
            {
                CriticalOtpPurpose.PasswordReset => OtpPurpose.PasswordReset,
                CriticalOtpPurpose.HighAmountTransfer => OtpPurpose.HighAmountTransfer,
                CriticalOtpPurpose.CloseAccount => OtpPurpose.CloseAccount,
                CriticalOtpPurpose.DeleteCard => OtpPurpose.DeleteCard,
                _ => throw new ArgumentOutOfRangeException()
            };

            string strPurpose = purpose.ToString().ToLower();

            if (await _otpService.IsTooManyAttempsAsync(userEmail, purpose))
                return Result.Failure(AuthError.TooManyRequests);

            string otpKey = $"otp:{strPurpose}:{userEmail.ToLower()}";


            string? otp = await _casheService.GetAsync<string>(otpKey);

            if (otp == null)
            {
                return Result.Failure(new Error("Confirm.Failure",
                                                "The Code was Expired!",
                                                ErrorType.Failure));
            }

            Guid userId = _currentUser.GetUserId();

            string attempKey = $"otp:{strPurpose}:attempts:{userEmail.ToLower()}";
            string rateLimitKey = $"otp:{strPurpose}:rate-limit:{userEmail.ToLower()}";


            if (otp == request.OtpCode)
            {
                await _casheService.SetAsync($"otp-confirmed:{strPurpose}:{userId}"
                                              ,"1", TimeSpan.FromMinutes(EXP_MINUTE));

                await _casheService.DeleteAsync(otpKey);
                await _casheService.DeleteAsync(attempKey);
                await _casheService.DeleteAsync(rateLimitKey);
            }
            else
                return Result.Failure(new Error("Confirm.Failure",
                                                "The Code is Wrong!",
                                                ErrorType.Failure));

            return Result.Success();
        }
    }
}
