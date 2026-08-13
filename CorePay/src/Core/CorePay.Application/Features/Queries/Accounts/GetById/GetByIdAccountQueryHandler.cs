using AutoMapper;
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

namespace CorePay.Application.Features.Queries.Accounts.GetById
{
    public class GetByIdAccountQueryHandler : IRequestHandler<GetByIdAccountQuery, Result<GetByIdAccountResponse>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public GetByIdAccountQueryHandler(IAccountRepository accountRepository,
                                          IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }
        public async Task<Result<GetByIdAccountResponse>> Handle(GetByIdAccountQuery request, CancellationToken cancellationToken)
        {
            Account? account = await _accountRepository.GetByIdAsync(request.Id);

            if (account == null)
                return Result<GetByIdAccountResponse>.Failure(AccountError.NotFound);

            return Result<GetByIdAccountResponse>.Success(_mapper.Map<GetByIdAccountResponse>(account));

        }
    }
}
