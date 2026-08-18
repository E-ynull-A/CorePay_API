using CorePay.Application.Common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Domain.Entities;
using CorePay.Domain.Utilities.Errors;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Cards.Remove
{
    public class RemoveCardCommandHandler : IRequestHandler<RemoveCardCommand, Result>
    {
        private readonly ICardRepository _cardRepository;

        public RemoveCardCommandHandler(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }
        public async Task<Result> Handle(RemoveCardCommand request, CancellationToken cancellationToken)
        {
            Card? card = await _cardRepository.GetByIdAsync(request.Id);

            if (card is null)
                return Result.Failure(CardError.NotFound);

            card.SoftDelete();
            _cardRepository.Update(card);


            await _cardRepository.SaveChangesAsync();

            return Result.Success();        
        }
    }
}
