using CorePay.Application.Common;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Auth.OtpConfirm.Send
{
    public class SendOtpConfirmCommandHandler : IRequestHandler<SendOptConfirmCommand, Result>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IEmailService _emailService;
        private readonly IRedisCasheService _redisCashe;
        private readonly IOtpService _otpService;
        private const double EXP_MINUTE_OTP = 3.5;
        public SendOtpConfirmCommandHandler(ICurrentUserService currentUser,
                                            IEmailService emailService,
                                            IRedisCasheService redisCashe,
                                            IOtpService otpService)
        {
            _currentUser = currentUser;
            _emailService = emailService;
            _redisCashe = redisCashe;
            _otpService = otpService;
        }
        public async Task<Result> Handle(SendOptConfirmCommand request, CancellationToken cancellationToken)
        {
            string email = _currentUser.GetUserEmail();

            OtpPurpose purpose = request.Purpose switch
            {
                CriticalOtpPurpose.PasswordReset => OtpPurpose.PasswordReset,
                CriticalOtpPurpose.HighAmountTransfer => OtpPurpose.HighAmountTransfer,
                CriticalOtpPurpose.CloseAccount => OtpPurpose.CloseAccount,
                CriticalOtpPurpose.DeleteCard => OtpPurpose.DeleteCard,               
                _ => throw new ArgumentOutOfRangeException()
            };

           Result result = await _otpService
                            .SendConfirmOtpAsync(email,purpose,EXP_MINUTE_OTP);

           return result;
        }
    }
}
