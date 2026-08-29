using CorePay.Application.Common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Entities;
using CorePay.Domain.Exceptions;
using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors;
using CorePay.Domain.Utilities.Errors.Common;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Transactions.ATM.Withdraw
{
    public class WithdrawCommandHandler : IRequestHandler<WithdrawCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisCasheService _casheService;

        public WithdrawCommandHandler(IUnitOfWork unitOfWork,
                                      IRedisCasheService casheService)
        {
            _unitOfWork = unitOfWork;
            _casheService = casheService;
        }
        public async Task<Result> Handle(WithdrawCommand request, CancellationToken cancellationToken)
        {
            Guid cardId = await _casheService.GetAsync<Guid>
                                    ($"atm-sessionId:{request.SessionId}");

            if (cardId == default)
                return Result.Failure(new Error("ATM.ExpiredSessionId",
                                                "ATM Session was Expired!",
                                                ErrorType.Unauthorized));

            Account? account = await _unitOfWork.AccountRepository
                                            .FirstOrDefaultAsync(a => a.Cards
                                                            .Any(c => c.Id == cardId));

            if (account is null)
                return Result.Failure(AccountError.NotFound);

            if (account.Balance < request.Amount)
                return Result.Failure(TransactionError.NoEnoughBalance);


            account.DecreaseBalance(request.Amount);
            _unitOfWork.AccountRepository.Update(account);

            using var transactionDb = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                Transaction withdraw = new Transaction(request.Amount,
                                                       TransactionType.Withdraw,
                                                       account.Id,
                                                       cardId); 

                _unitOfWork.TransactionRepository.Add(withdraw);
                await _unitOfWork.SaveChangeAsync();

                await transactionDb.CommitAsync(cancellationToken);
                return Result.Success();
            }
            catch(Exception ex)
            {
               await transactionDb.RollbackAsync(cancellationToken);

               throw new TransactionException("Unexpexted error was occured!", ex);
            }
        }
    }
}
