using AutoMapper;
using CorePay.Application.Common;
using CorePay.Domain.Entities;
using CorePay.Domain.Exceptions;
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
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;

        public RegisterCommandHandler(UserManager<AppUser> userManager,
                                      SignInManager<AppUser> signInManager,
                                      IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }
        public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await _userManager.Users.AnyAsync(u => u.Email == request.Email
                                        || u.UserName == request.Username))
                return Result.Failure(AuthError.Dublicate);

            IdentityResult result = await _userManager
                             .CreateAsync(_mapper.Map<AppUser>(request),
                                          request.Password);

            if (!result.Succeeded)
                throw new IdentityException(result.Errors);

            return Result.Success();
        }
    }
}
