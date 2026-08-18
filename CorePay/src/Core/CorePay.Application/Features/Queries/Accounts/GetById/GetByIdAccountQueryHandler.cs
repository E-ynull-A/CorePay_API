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

namespace CorePay.Application.Features.Queries.Accounts.GetById
{
    public class GetByIdAccountQueryHandler : IRequestHandler<GetByIdAccountQuery, Result<GetByIdAccountResponse>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public GetByIdAccountQueryHandler(IAccountRepository accountRepository,
                                          IMapper mapper,
                                          ICurrentUserService currentUser)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _currentUser = currentUser;
        }
        public async Task<Result<GetByIdAccountResponse>> Handle(GetByIdAccountQuery request, CancellationToken cancellationToken)
        {
            Guid userId = _currentUser.GetUserId();

            Account? account = await _accountRepository.FirstOrDefaultAsync(a=>a.Id == request.Id && a.AppUserId == userId);

            if (account == null)
                return Result<GetByIdAccountResponse>.Failure(AccountError.NotFound);

            return Result<GetByIdAccountResponse>.Success(_mapper.Map<GetByIdAccountResponse>(account));

        }
    }
}
