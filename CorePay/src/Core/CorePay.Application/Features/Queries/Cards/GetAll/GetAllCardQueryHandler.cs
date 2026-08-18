using AutoMapper;
using CorePay.Application.Common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Queries.Cards.GetAll
{
    public class GetAllCardQueryHandler : IRequestHandler<GetAllCardQuery, Result<ICollection<GetAllCardResponse>>>
    {
        private readonly ICardRepository _cardRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public GetAllCardQueryHandler(ICardRepository cardRepository,
                                      ICurrentUserService currentUser,
                                      IMapper mapper)
        {
            _cardRepository = cardRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }
        public async Task<Result<ICollection<GetAllCardResponse>>> Handle(GetAllCardQuery request, CancellationToken cancellationToken)
        {
            Guid userId = _currentUser.GetUserId();

            ICollection<Card> cards = await _cardRepository.GetAll(func: c => c.Account.AppUserId == userId
                                                                  ,page: request.Page
                                                                  ,take: request.Take).ToListAsync();

            return Result<ICollection<GetAllCardResponse>>.Success
                (_mapper.Map<ICollection<GetAllCardResponse>>(cards));
        }
    }
}
