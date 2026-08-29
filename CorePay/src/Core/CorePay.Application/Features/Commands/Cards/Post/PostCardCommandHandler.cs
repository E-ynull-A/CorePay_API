using CorePay.Application.Common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Entities;
using CorePay.Domain.Utilities.Errors;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace CorePay.Application.Features.Commands.Cards.Post
{
    public class PostCardCommandHandler : IRequestHandler<PostCardCommand, Result>
    {
        private readonly ICardRepository _cardRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ISystemValueGeneratorService _generatorService;

        public PostCardCommandHandler(ICardRepository cardRepository,
                                      IAccountRepository accountRepository,
                                      ISystemValueGeneratorService generatorService)
        {
            _cardRepository = cardRepository;
            _accountRepository = accountRepository;
            _generatorService = generatorService;
        }
        public async Task<Result> Handle(PostCardCommand request, CancellationToken cancellationToken)
        {

            if (!await _accountRepository.AnyAsync(a => a.Id == request.AccountId))
                return Result.Failure(AccountError.NotFound);

            if (await _accountRepository.AnyAsync(a => a.Cards.Count == 5 
                                                    && a.Id == request.AccountId))
                return Result.Failure(CardError.ReachedCardLimit);

            var hasher = new PasswordHasher<object>();

            Card card = new Card(
                await _generatorService.GenerateCardNumberAsync(),
                DateOnly.FromDateTime(DateTime.Now.AddYears(3)),
                _generatorService.GenerateCvnCode(),
                request.AccountId,
                hasher.HashPassword(null!,request.PIN));

            _cardRepository.Add(card);
            await _cardRepository.SaveChangesAsync();

            return Result.Success();
        }

    }
}
