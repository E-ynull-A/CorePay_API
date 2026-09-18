using CorePay.Application.Common;
using CorePay.Application.Features.Commands.Transactions.MobileApp.common;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Infrastructure.Services
{
    public class TransferService:ITransferService
    {
        private readonly IRedisCasheService _redisCashe;
        private readonly ICurrentUserService _currentUser;
        private readonly IOtpService _otpService;

        public TransferService(IRedisCasheService redisCashe,
                               ICurrentUserService currentUser,
                               IOtpService otpService)
        {
            _redisCashe = redisCashe;
            _currentUser = currentUser;
            _otpService = otpService;
        }
        public async Task<Result> HighAmountTransferAsync(Guid userId,
                                                         string sessionId,
                                                         PendingTransferContext context)
        {
            if (!await _redisCashe.AnyAsync($"otp-confirmed:{OtpPurpose.HighAmountTransfer.ToString().ToLower()}:{userId}:{sessionId}"))
            {
                string? email = _currentUser.GetUserEmail();

                if (email is null)
                    return Result.Failure(AuthError.NotFound);

                Result<string> result = await _otpService.SendConfirmOtpAsync(email, OtpPurpose.HighAmountTransfer, 3);

                await _redisCashe.SetAsync($"pending-transfer:{userId.ToString()}:{result.Value}",
                                              new PendingTransferContext(context.SenderAccountId,
                                                                         context.ReceiverAccountId,
                                                                         context.Amount)
                                              , TimeSpan.FromMinutes(4));

                return Result.Failure(TransactionError.OtpRequired with { Details = result.Value });
            }
            return Result.Success();
        }

        public async Task<Result> CheckTransferContextAsync(Guid userId,
                                                            string sessionId,
                                                            PendingTransferContext context)
        {
            PendingTransferContext? transferContext = await _redisCashe.GetAsync<PendingTransferContext>
                                        ($"pending-transfer:{userId.ToString()}:{sessionId}");

            if (transferContext == default
              || transferContext.Amount != context.Amount
              || transferContext.SenderAccountId != context.SenderAccountId
              || transferContext.ReceiverAccountId != context.ReceiverAccountId)
                return Result.Failure(TransactionError.InvalidOtpContext);

            await _redisCashe.DeleteAsync($"otp-confirmed:{OtpPurpose.HighAmountTransfer}:{userId}");

            return Result.Success();
        }
    }
}
