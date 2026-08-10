using AutoMapper;
using CorePay.Application.Common;
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
        private readonly IMapper _mapper;

        public RegisterCommandHandler(UserManager<AppUser> userManager,
                                      IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
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

            await _userManager.AddToRoleAsync(user, Role.User.ToString());

            IdentityResult result = await _userManager.CreateAsync(user,request.Password);

            if (!result.Succeeded)
                throw new IdentityException(result.Errors);

            return Result.Success();
        }
    }
}
