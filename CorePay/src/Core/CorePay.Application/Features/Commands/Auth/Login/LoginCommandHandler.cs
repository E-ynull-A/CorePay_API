using CorePay.Application.Common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Entities;
using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Auth.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand,Result<LoginCommandResponce>>
    {
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenRepository _tokenRepository;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;

        public LoginCommandHandler(ITokenService tokenService,
                                   IRefreshTokenRepository tokenRepository,
                                   SignInManager<AppUser> signInManager,
                                   IConfiguration configuration)
        {
            _tokenService = tokenService;
            _tokenRepository = tokenRepository;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        public async Task<Result<LoginCommandResponce>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {

            AppUser? user = await _signInManager.UserManager.Users.FirstOrDefaultAsync(u => u.Email == request.UsernameOrEmail ||
                                                                     u.UserName == request.UsernameOrEmail);
            if(user is null)
                return new Result<LoginCommandResponce>(AuthError.NotFound);               

            SignInResult result = await _signInManager
                .CheckPasswordSignInAsync(user,request.Password,user.LockoutEnabled);

            if (result.IsLockedOut)
                return new Result<LoginCommandResponce>(AuthError.AccountLockedOut);

            if (!result.Succeeded)
                return new Result<LoginCommandResponce>(AuthError.InvalidCredentials);

            double expireTime = double.TryParse(_configuration["RefreshToken:ExpireTime"], out double time) ? time : 0;

            if (expireTime == 0)
                throw new InvalidOperationException("JWT:ExpireTime configuration is invalid.");

            RefreshToken refreshToken = new RefreshToken(
                                _tokenService.GenerateRefreshToken(),
                                user.Id,
                                DateTimeOffset.UtcNow.AddDays(expireTime));

            _tokenRepository.Add(refreshToken);
            await _tokenRepository.SaveChangesAsync();

            LoginCommandResponce responce = new LoginCommandResponce(
                _tokenService.GenerateAccessToken(user,
                                    (await _signInManager.UserManager
                                        .GetRolesAsync(user))
                                            .ToArray()),
                refreshToken.Token);

            return new Result<LoginCommandResponce>(responce);
        }
    }
}
