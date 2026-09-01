using CorePay.Application.Common;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Utilities.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Auth.EmailConfirm.Confirm
{
    public class ConfirmEmailResendCommandHandler : IRequestHandler<ConfirmEmailResendCommand, Result>
    {
        private readonly IOtpService _otpService;

        public ConfirmEmailResendCommandHandler(IOtpService confirmService)
        {
            _otpService = confirmService;
        }
        public async Task<Result> Handle(ConfirmEmailResendCommand request, CancellationToken cancellationToken)
        {
            Result emailResult = await _otpService
                                            .SendConfirmOtpAsync(request.Email,OtpPurpose.EmailConfirm,3);

            if (!emailResult.IsSuccess)
                return emailResult;

            return Result.Success();
        }
    }
}
