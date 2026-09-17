using CorePay.Application.Common;
using CorePay.Application.Features.Commands.Transactions.MobileApp.common;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors;
using System.Security.Cryptography;

namespace CorePay.Infrastructure.Services
{
    public class OtpService : IOtpService
    {
        private readonly IEmailService _emailService;
        private readonly IRedisCasheService _redisCashe;
        private readonly ICurrentUserService _currentUser;

        public OtpService(IEmailService emailService,
                          IRedisCasheService redisCashe,
                          ICurrentUserService currentUser)
        {
            _emailService = emailService;
            _redisCashe = redisCashe;
            _currentUser = currentUser;
        }

        public async Task<Result<string>> SendConfirmOtpAsync(string toEmail
                                                      ,OtpPurpose purpose
                                                      ,double expireMinute)
        {
            int code = RandomNumberGenerator.GetInt32(100000, 999999);

            string processId = Guid.NewGuid().ToString();

            if (await _redisCashe.CountAsync($"otp:{purpose.ToString().ToLower()}:rate-limit:{toEmail.ToLower()}:processId:{processId}",
                                                TimeSpan.FromMinutes(10)) > 3)
                return Result<string>.Failure(AuthError.TooManyRequests);

            string otpKey = $"otp:{purpose.ToString().ToLower()}:{toEmail.ToLower()}:{processId}";

            if (await _redisCashe.AnyAsync(otpKey))
                await _redisCashe.DeleteAsync(otpKey);

            await _redisCashe.SetAsync(otpKey,code, 
                                TimeSpan.FromMinutes(expireMinute));

            (string subject, string actionDescription) = purpose switch
            {
                OtpPurpose.EmailConfirm => ("Verify Your Email",
                                            "To complete your registration and verify your email address," +
                                            " please enter the following verification code:"),
                OtpPurpose.PasswordReset => ("Reset Your Password",
                                             "We received a request to reset your password." +
                                             " Please enter the following code to proceed:"),
                OtpPurpose.HighAmountTransfer => ("Confirm Your Transaction",
                                                "You are performing a high-value transaction on CorePay." +
                                                " Please enter the following OTP to confirm:"),
                _ => ("Verification Code",
                    "Please use the following verification code to complete your action:")
            };

            string body = $"""
                
                          Hello,
                
                          Welcome to CorePay!

                          {actionDescription}

                          👉  {code}  👈

                          This verification code is valid for {expireMinute - 0.5} minutes. Please do not share this code with anyone.

                          If you did not create a CorePay account, you can safely ignore this email.

                          Best regards,
                          CorePay Team
                
                          """;


            await _emailService.SendEmailAsync(toEmail, subject, body);
            return Result<string>.Success(processId);
        }

        public async Task<bool> IsTooManyAttempsAsync(string email,
                                                      OtpPurpose purpose,
                                                      string processId)
        {
            if (await _redisCashe.CountAsync($"otp:{purpose.ToString().ToLower()}:attempts:{email.ToLower()}:processId:{processId}"
                                                    ,TimeSpan.FromMinutes(10)) >= 4)
                return true;

            return false;
        }

        
    }
}
