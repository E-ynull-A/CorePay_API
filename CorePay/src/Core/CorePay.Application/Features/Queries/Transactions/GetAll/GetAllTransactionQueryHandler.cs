using AutoMapper;
using CorePay.Application.Common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Entities;
using CorePay.Domain.Utilities.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace CorePay.Application.Features.Queries.Transactions.GetAll
{
    public class GetAllTransactionQueryHandler : IRequestHandler<GetAllTransactionQuery, Result<ICollection<GetAllTransactionQueryResponse>>>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrentUserService _userService;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public GetAllTransactionQueryHandler(ITransactionRepository transactionRepository,
                                             ICurrentUserService userService,
                                             IAccountRepository accountRepository,
                                             IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _userService = userService;
            _accountRepository = accountRepository;
            _mapper = mapper;
        }
        public async Task<Result<ICollection<GetAllTransactionQueryResponse>>> Handle(GetAllTransactionQuery request, CancellationToken cancellationToken)
        {
            Guid userId = _userService.GetUserId();                  

            Account? account = await _accountRepository
                                    .FirstOrDefaultAsync(t => t.Id == request.AccountId
                                                           && t.AppUserId == userId);

            if (account == null)
                return Result<ICollection<GetAllTransactionQueryResponse>>
                                                    .Failure(AccountError.NotFound);



            ICollection<Transaction> transactions = await _transactionRepository
                                                                        .GetAll(t => t.AccountId == account.Id,
                                                                                  page: request.page, take: request.take)
                                                                                .ToListAsync();


           return Result<ICollection<GetAllTransactionQueryResponse>>
                            .Success(_mapper.Map<ICollection<GetAllTransactionQueryResponse>>
                                                        (transactions, 
                                                         otp => otp
                                                                .Items[nameof(Account.Currency)] = account.Currency));

        }
    }
}
