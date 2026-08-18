using CorePay.Application.Common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Domain.Entities;
using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Cards.Lost
{
    public class LockCardToggleCommandHandler : IRequestHandler<LockCardCommand, Result>
    {
        private readonly ICardRepository _cardRepository;

        public LockCardToggleCommandHandler(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }
        public async Task<Result> Handle(LockCardCommand request, CancellationToken cancellationToken)
        {
            Card? card = await _cardRepository.GetByIdAsync(request.Id);

            if (card is null)
                return Result.Failure(CardError.NotFound);

            switch (card.Status)
            {
                case CardStatus.Active:
                    card.Lock();
                    break;
                case CardStatus.Blocked:
                    return Result.Failure(CardError.Bloked);
                case CardStatus.Expired:
                    return Result.Failure(CardError.Expired);
                case CardStatus.Locked:
                    card.Active();
                    break;
            }

            _cardRepository.Update(card);
            await _cardRepository.SaveChangesAsync();

            return Result.Success();
        }
    }
}
