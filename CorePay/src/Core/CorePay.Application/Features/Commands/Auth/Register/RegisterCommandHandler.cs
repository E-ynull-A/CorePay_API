using AutoMapper;
using CorePay.Application.Common;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Entities;
using CorePay.Domain.Exceptions;
using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors;
using CorePay.Domain.Utilities.Errors.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Auth.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailConfirmService _confirmService;

        public RegisterCommandHandler(UserManager<AppUser> userManager,
                                      IEmailConfirmService confirmService)
        {
            _userManager = userManager;
            _confirmService = confirmService;
        }
        public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await _userManager.Users.AnyAsync(u => u.Email == request.Email
                                        || u.UserName == request.Username))
                return Result.Failure(AuthError.Dublicate);


            AppUser user = new AppUser(request.Name,
                                       request.Surname,
                                       request.Username,
                                       request.Birthdate,
                                       request.Email,
                                       request.PhoneNumber,
                                       request.FIN);

            

            IdentityResult result = await _userManager.CreateAsync(user,request.Password);

            if (!result.Succeeded)
                throw new IdentityException(result.Errors);

            IdentityResult roleResult = await _userManager.AddToRoleAsync(user, Role.User.ToString());

            if (!roleResult.Succeeded)
                throw new IdentityException(roleResult.Errors);

            Result emailResult = await _confirmService
                                            .SendConfirmEmailAsync(request.Email);

            if (!emailResult.IsSuccess)
                return emailResult;

            return Result.Success();
        }
    }
}
