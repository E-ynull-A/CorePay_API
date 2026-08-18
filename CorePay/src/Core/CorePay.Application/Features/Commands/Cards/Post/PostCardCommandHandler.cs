using CorePay.Application.Common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Entities;
using CorePay.Domain.Utilities.Errors;
using MediatR;

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

            if (await _accountRepository.AnyAsync(a => a.Cards.Count == 5))
                return Result.Failure(CardError.ReachedCardLimit);

            Card card = new Card(
                _generatorService.GenerateCardNumber(),
                DateOnly.FromDateTime(DateTime.Now.AddYears(3)),
                _generatorService.GenerateCvnCode(),
                request.AccountId);

            _cardRepository.Add(card);
            await _cardRepository.SaveChangesAsync();

            return Result.Success();
        }

    }
}
