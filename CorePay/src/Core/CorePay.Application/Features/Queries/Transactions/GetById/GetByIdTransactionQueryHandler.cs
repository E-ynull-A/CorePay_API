using CorePay.Application.Common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Entities;
using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors;
using MediatR;
using System.Linq.Expressions;
using CorePay.Domain.Utilities.Extentions;
using AutoMapper;

namespace CorePay.Application.Features.Queries.Transactions.GetById
{
    public class GetByIdTransactionQueryHandler : IRequestHandler<GetByIdTransactionQuery, Result<GetByIdTransactionQueryResponse>>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransferRepository _transferRepository;
        private readonly IMapper _mapper;

        public GetByIdTransactionQueryHandler(ITransactionRepository transactionRepository,
                                              ICurrentUserService currentUser,
                                              IAccountRepository accountRepository,
                                              ITransferRepository transferRepository,
                                              IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _currentUser = currentUser;
            _accountRepository = accountRepository;
            _transferRepository = transferRepository;
            _mapper = mapper;
        }
        public async Task<Result<GetByIdTransactionQueryResponse>> Handle(GetByIdTransactionQuery request, CancellationToken cancellationToken)
        {
            Guid userId = _currentUser.GetUserId();

            Transaction? transaction = await _transactionRepository.FirstOrDefaultAsync(t=>t.Id == request.Id && userId 
                                                                                                == t.Account.AppUserId,
                                                                                                [nameof(Transaction.Transfer),
                                                                                                 nameof(Transaction.Account)]);

            if (transaction == null)
                return Result<GetByIdTransactionQueryResponse>
                            .Failure(TransactionError.NotFound);

            GetTransferInGetByIdTransactionQuery transfer = default;

            if (transaction.Transfer is not null)
            {
                string? counterAccountIBAN = string.Empty;
                string? counterCardNumber = string.Empty;
                Expression<Func<Transfer,string>>? fullNameExpression = null;
                TransferType type = default;

                if (await _transferRepository.AnyAsync(t=> t.SenderAccountId == transaction.Transfer.SenderAccountId
                                                        ||
                                                       (t.SenderCardId != null 
                                                       && transaction.Transfer.SenderCardId != null 
                                                       && t.SenderCard.AccountId == transaction.Transfer.SenderCard.AccountId)))
                {

                    counterCardNumber = await _transferRepository
                                                    .GetPropertyAsync(t => t.Id == transaction.TransferId,
                                                                      t => t.RecieverCard.CardNumber);
                    
                    if (string.IsNullOrWhiteSpace(counterCardNumber))
                    {
                        counterAccountIBAN = await _transferRepository
                                                    .GetPropertyAsync(t => t.Id == transaction.TransferId,
                                                                      t => t.RecieverAccount.IBAN);

                        if(string.IsNullOrWhiteSpace(counterAccountIBAN))
                            new InvalidOperationException(
                                $"Corrupted transfer data detected for TransferId: '{transaction.TransferId}'." +
                                $"Both of two credentials (Card Number,IBAN) are Null!");

                        fullNameExpression = (t) => string.Concat(t.RecieverAccount.AppUser.Name,
                                                                    " "
                                                                 , t.RecieverAccount.AppUser.Surname);

                        type = TransferType.ToIBAN;
                    }


                    else
                    {
                        fullNameExpression = (t) => string.Concat(t.RecieverCard.Account.AppUser.Name,
                                                                   " "
                                                                , t.RecieverCard.Account.AppUser.Name);
                        type = TransferType.CardToCard; 
                    }
                       

                }
                else
                {
                    counterCardNumber = await _transferRepository
                                                    .GetPropertyAsync(t => t.Id == transaction.TransferId,
                                                                      t => t.SenderCard.CardNumber);

                    if (string.IsNullOrWhiteSpace(counterCardNumber))
                    {
                        counterAccountIBAN = await _transferRepository
                                                   .GetPropertyAsync(t => t.Id == transaction.TransferId,
                                                                      t => t.SenderAccount.IBAN);

                        if (string.IsNullOrWhiteSpace(counterAccountIBAN))
                            new InvalidOperationException(
                                $"Corrupted transfer data detected for TransferId: '{transaction.TransferId}'." +
                                $"Both of two credentials (Card Number,IBAN) are Null!");

                        fullNameExpression = (t) => string.Concat(t.SenderAccount.AppUser.Name,
                                                                    " "
                                                                 , t.SenderAccount.AppUser.Surname);

                        type = TransferType.ToIBAN;
                    }
                    else
                    {
                        fullNameExpression = (t) => string.Concat(t.SenderCard.Account.AppUser.Name,
                                                                    " "
                                                                 , t.SenderCard.Account.AppUser.Name);
                        type = TransferType.CardToCard;
                    }

                }



                string? counterFullName = await _transferRepository.GetPropertyAsync(t=>t.Id == transaction.TransferId,
                                                                                            fullNameExpression);

                if (string.IsNullOrWhiteSpace(counterFullName))
                    return Result<GetByIdTransactionQueryResponse>
                                    .Failure(AuthError.NotFound);

                string counterData = string.Empty;

                if (type == TransferType.CardToCard)
                    counterData = counterCardNumber.MaskCardNumber();
                else
                    counterData = counterAccountIBAN.MaskIban();

                transfer = new(counterFullName, counterData, type);
            }

            GetByIdTransactionQueryResponse response = _mapper.Map<GetByIdTransactionQueryResponse>
                                                                                (transaction, otp =>
                                                                                {
                                                                                    otp.Items[nameof(Account.Currency)] = transaction.Account.Currency;
                                                                                    otp.Items[nameof(GetByIdTransactionQueryResponse.Transfer)] = transfer;
                                                                                });

            return Result<GetByIdTransactionQueryResponse>
                                        .Success(response);

        }
    }
}
