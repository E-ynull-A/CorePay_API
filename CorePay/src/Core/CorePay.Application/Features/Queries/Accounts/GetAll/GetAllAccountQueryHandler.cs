using AutoMapper;
using CorePay.Application.Common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CorePay.Application.Features.Queries.Accounts.GetAll
{
    public class GetAllAccountQueryHandler : IRequestHandler<GetAllAccountQuery, Result<ICollection<GetAllAccountQueryResponse>>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public GetAllAccountQueryHandler(IAccountRepository accountRepository,
                                          IMapper mapper,
                                          ICurrentUserService currentUser)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _currentUser = currentUser;
        }
        public async Task<Result<ICollection<GetAllAccountQueryResponse>>> Handle(GetAllAccountQuery request, CancellationToken cancellationToken)
        {
            Guid userId = _currentUser.GetUserId();

            ICollection<Account> accounts = await _accountRepository.GetAll(func: a => a.AppUserId == userId,
                                                                            page: request.Page, take: request.Take).ToListAsync();

            return Result<ICollection<GetAllAccountQueryResponse>>.Success(_mapper.Map<ICollection<GetAllAccountQueryResponse>>(accounts));
        }
    }
}
