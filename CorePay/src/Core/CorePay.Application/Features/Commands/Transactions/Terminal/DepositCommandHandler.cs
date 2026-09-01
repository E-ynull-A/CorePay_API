using CorePay.Application.Common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Domain.Entities;
using CorePay.Domain.Exceptions;
using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Transactions.Deposit
{
    public class DepositCommandHandler : IRequestHandler<DepositCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepositCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(DepositCommand request, CancellationToken cancellationToken)
        {
            Card? card = await _unitOfWork.CardRepository
                                    .FirstOrDefaultAsync(c => c.CardNumber == request.CardNumber,
                                                         [nameof(Account)]);

            if (card == null)
                return Result.Failure(CardError.NotFound);

            if(card.IsExpired())
                return Result.Failure(CardError.Expired);

            if (card.Status != CardStatus.Active
                || card.Account.Status != AccountStatus.Active)
            return Result.Failure(TransactionError.InvalidStatus);

            card.Account.IncreaseBalance(request.Amount);
            _unitOfWork.AccountRepository.Update(card.Account);

            using var trasactionDb = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                Transaction transaction = new Transaction(request.Amount,
                                                          TransactionType.Deposit,
                                                          card.AccountId,
                                                          card.Id);

                _unitOfWork.TransactionRepository.Add(transaction);
                await _unitOfWork.SaveChangeAsync();

                await trasactionDb.CommitAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                await trasactionDb.RollbackAsync(cancellationToken);
                throw new TransactionException("Unexpexted error was occured!",ex);
            }
        }
    }
}
