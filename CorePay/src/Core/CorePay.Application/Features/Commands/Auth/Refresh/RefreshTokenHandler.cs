using CorePay.Application.Common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Entities;
using CorePay.Domain.Utilities.Errors;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Auth.Refresh
{
    public class RefreshTokenHandler : IRequestHandler<RefreshCommand, Result<RefreshCommandResponse>>
    {
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenRepository _tokenRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public RefreshTokenHandler(ITokenService tokenService
                                  ,IRefreshTokenRepository tokenRepository,
                                   UserManager<AppUser> userManager,
                                   IConfiguration configuration)
        {
            _tokenService = tokenService;
            _tokenRepository = tokenRepository;
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<Result<RefreshCommandResponse>> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            RefreshToken? refreshToken = await _tokenRepository.FirstOrDefaultAsync(t => t.Token == request.RefreshToken, [nameof(RefreshToken.AppUser)]);

            if (refreshToken is null)
                return Result<RefreshCommandResponse>.Failure(TokenError.NotFound);

            if (refreshToken.IsRevoked)
                return Result<RefreshCommandResponse>.Failure(TokenError.Revoked);

            if (refreshToken.ValidFrom < DateTimeOffset.UtcNow)
                return Result<RefreshCommandResponse>.Failure(TokenError.Expired);


            string accessToken = _tokenService.GenerateAccessToken(refreshToken.AppUser,
                                                                  (await _userManager
                                                                        .GetRolesAsync(refreshToken.AppUser))
                                                                        .ToArray());

            double expireTime = double.TryParse(_configuration["RefreshToken:ExpireTime"], out double time) ? time : 0;

            if (expireTime == 0)
                throw new InvalidOperationException("JWT:ExpireTime configuration is invalid.");

            RefreshToken newRefreshTkn = new(_tokenService.GenerateRefreshToken()
                                            ,refreshToken.AppUserId
                                            ,DateTimeOffset.UtcNow.AddDays(expireTime));



            _tokenRepository.Add(newRefreshTkn);

            refreshToken.Revoke();
            refreshToken.SoftDelete(Guid.Parse("af29e40f-ff70-4d9a-1615-08def795fab3"));
            _tokenRepository.Update(refreshToken);
            

            RefreshCommandResponse response = new RefreshCommandResponse(accessToken,newRefreshTkn.Token);

            await _tokenRepository.SaveChangesAsync();

            return Result<RefreshCommandResponse>.Success(response);
        }
    }
}
