using CorePay.Application.Common;
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

        public OtpService(IEmailService emailService, IRedisCasheService redisCashe)
        {
            _emailService = emailService;
            _redisCashe = redisCashe;
        }

        public async Task<Result> SendConfirmOtpAsync(string toEmail
                                                      ,OtpPurpose purpose
                                                      ,int expireMinute)
        {
            int code = RandomNumberGenerator.GetInt32(100000, 999999);

            if (await _redisCashe.CountAsync($"otp:{purpose.ToString().ToLower()}:rate-limit:{toEmail.ToLower()}",
                                                TimeSpan.FromMinutes(10)) > 3)
                return Result.Failure(AuthError.TooManyRequests);

            string otpKey = $"otp:{purpose.ToString().ToLower()}:{toEmail.ToLowerInvariant()}";

            if (await _redisCashe.AnyAsync(otpKey))
                await _redisCashe.DeleteAsync(otpKey);

            await _redisCashe.SetAsync($"otp:{purpose.ToString().ToLower()}:{toEmail.ToLowerInvariant()}",
                                    code, TimeSpan.FromMinutes(expireMinute));

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

                          This verification code is valid for {expireMinute} minutes. Please do not share this code with anyone.

                          If you did not create a CorePay account, you can safely ignore this email.

                          Best regards,
                          CorePay Team
                
                          """;


            await _emailService.SendEmailAsync(toEmail, subject, body);
            return Result.Success();
        }

        public async Task<bool> IsTooManyAttempsAsync(string email, OtpPurpose purpose)
        {

            if (await _redisCashe.CountAsync($"otp:{purpose.ToString().ToLower()}:attempts:{email.ToLower()}"
                                                    , TimeSpan.FromMinutes(10)) == 4)
                return true;

            return false;
        }
    }
}
