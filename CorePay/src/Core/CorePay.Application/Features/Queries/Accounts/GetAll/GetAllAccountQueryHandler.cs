using AutoMapper;
using CorePay.Application.Common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Entities;
using CorePay.Domain.Utilities.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CorePay.Domain.Utilities.Extentions;

namespace CorePay.Application.Features.Queries.Accounts.GetAll
{
    public class GetAllAccountQueryHandler : IRequestHandler<GetAllAccountQuery, Result<ICollection<GetAllAccountResponse>>>
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
        public async Task<Result<ICollection<GetAllAccountResponse>>> Handle(GetAllAccountQuery request, CancellationToken cancellationToken)
        {
            Guid userId = _currentUser.GetUserId();               

            ICollection<Account> accounts = await _accountRepository
                                                                .GetAll(func: a => a.AppUserId == userId)
                                                                    .ToListAsync();


            return Result<ICollection<GetAllAccountResponse>>.Success
                (_mapper.Map<ICollection<GetAllAccountResponse>>(accounts));
        }
    }
}
