using CorePay.Application.Common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Entities;
using CorePay.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace CorePay.Application.Features.Commands.Auth.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ICurrentUserService _currentUser;

        public LogoutCommandHandler(SignInManager<AppUser> signInManager,
                                    IRefreshTokenRepository refreshTokenRepository,
                                    ICurrentUserService currentUser)
        {
            _signInManager = signInManager;
            _refreshTokenRepository = refreshTokenRepository;
            _currentUser = currentUser;
        }
        public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            Guid userID = _currentUser.GetUserId();

            RefreshToken? refreshToken = await _refreshTokenRepository
                                             .FirstOrDefaultAsync(r => r.AppUserId == userID);

            if (refreshToken is not null)
            {
                refreshToken.Revoke();
                refreshToken.SoftDelete();

                _refreshTokenRepository.Update(refreshToken);
                await _refreshTokenRepository.SaveChangesAsync();
            }

            await _signInManager.SignOutAsync();
            return Result.Success();
        }
    }
}
