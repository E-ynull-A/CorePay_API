using CorePay.Application.Common;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Entities;
using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors;
using CorePay.Domain.Utilities.Errors.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CorePay.Application.Features.Commands.Auth.EmailConfirm
{
    public class EmailConfirmCommandHandler : IRequestHandler<EmailConfirmCommand, Result>
    {
        private readonly IRedisCasheService _redisCashe;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailConfirmService _emailConfirm;

        public EmailConfirmCommandHandler(IRedisCasheService redisCashe,
                                          UserManager<AppUser> userManager,
                                          IEmailConfirmService emailConfirm)
        {
            _redisCashe = redisCashe;
            _userManager = userManager;
            _emailConfirm = emailConfirm;
        }
        public async Task<Result> Handle(EmailConfirmCommand request, CancellationToken cancellationToken)
        {
            if (await _emailConfirm.IsTooManyAttempsAsync(request.Email,OtpPurpose.EmailConfirm))
                return Result.Failure(AuthError.TooManyRequests);

            if (await _redisCashe.GetAsync<string>($"otp:{OtpPurpose.EmailConfirm.ToString().ToLower()}:{request.Email.ToLowerInvariant()}") == request.Code)
            {
                AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (user == null)
                    return Result.Failure(AuthError.NotFound);

                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);

                await _redisCashe.DeleteAsync(request.Email);
                return Result.Success();
            }

            return Result.Failure(new Error("Confirm.Failure","The Code was expired Or Incorrect!",ErrorType.Failure));
        }
    }
}
