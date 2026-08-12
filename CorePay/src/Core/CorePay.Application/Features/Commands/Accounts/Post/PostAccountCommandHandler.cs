using CorePay.Application.Common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Entities;
using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CorePay.Application.Features.Commands.Accounts.Post
{
    public class PostAccountCommandHandler : IRequestHandler<PostAccountCommand, Result>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ISystemValueGeneratorService _generatorService;
        private readonly ICurrentUserService _currentUser;
        private readonly UserManager<AppUser> _userManager;

        public PostAccountCommandHandler(IAccountRepository accountRepository,
                                         ISystemValueGeneratorService generatorService,
                                         ICurrentUserService currentUser,
                                         UserManager<AppUser> userManager)
        {
            _accountRepository = accountRepository;
            _generatorService = generatorService;
            _currentUser = currentUser;
            _userManager = userManager;
        }
        public async Task<Result> Handle(PostAccountCommand request, CancellationToken cancellationToken)
        {
            Guid currentUserId = _currentUser.GetUserId();

            if (!await _userManager.Users.AnyAsync(u => u.Id == currentUserId))
                return Result.Failure(AuthError.NotFound);


            if (await _accountRepository.CountAsync(a => a.AppUserId == currentUserId) == 5)
                return Result.Failure(AccountError.ReachedAccountLimit);


            Account account = new Account(await _generatorService.GenerateIbanAsync()
                                         ,Currency.AZN,currentUserId);

            _accountRepository.Add(account);
            await _accountRepository.SaveChangesAsync();

            return Result.Success();
        }
    }
}
