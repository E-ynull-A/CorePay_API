using AutoMapper;
using CorePay.Application.Common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Entities;
using CorePay.Domain.Utilities.Errors;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Queries.Cards.GetById
{
    public class GetByIdCardQueryHandler : IRequestHandler<GetByIdCardQuery, Result<GetByIdCardResponse>>
    {
        private readonly ICardRepository _cardRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public GetByIdCardQueryHandler(ICardRepository cardRepository,
                                        ICurrentUserService currentUser,
                                        IMapper mapper)
        {
            _cardRepository = cardRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }
        public async Task<Result<GetByIdCardResponse>> Handle(GetByIdCardQuery request, CancellationToken cancellationToken)
        {
            Guid userId = _currentUser.GetUserId();

            Card? card = await _cardRepository.FirstOrDefaultAsync(c=>c.Id == request.Id 
                                                                    && c.Account.AppUserId == userId
                                                                    && c.AccountId == request.AccountId,
                                                                    includes: [nameof(Card.Account)]);

            if (card is null)
                return Result<GetByIdCardResponse>.Failure(CardError.NotFound);

            return Result<GetByIdCardResponse>.Success(_mapper.Map<GetByIdCardResponse>(card));
        }
    }
}
