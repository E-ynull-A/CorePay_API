using CorePay.Application.Common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Entities;
using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Transactions
{
    public class PostTransactionCommandHandler : IRequestHandler<PostTransactionCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public PostTransactionCommandHandler(IUnitOfWork unitOfWork,
                                             ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }
        public async Task<Result> Handle(PostTransactionCommand request, CancellationToken cancellationToken)
        {
            Guid userID = _currentUser.GetUserId();

            Account? senderAccount = default;
            Account? recieverAccount = default;

            Card? senderCard = default;
            Card? recieverCard = default;

            if (request.Type == TransactionType.Transfer
                || request.Type == TransactionType.Withdraw)
            {

                if (request.senderAccount is not null)
                {
                    senderAccount = await _unitOfWork.AccountRepository
                                       .FirstOrDefaultAsync(a => a.AppUserId == userID
                                                              && a.IBAN == request.senderAccount);

                    if (senderAccount is null)
                        return Result.Failure(AccountError.NotFound);
       
                }
                else
                {
                    senderCard = await _unitOfWork.CardRepository
                                        .FirstOrDefaultAsync(c => c.Account.AppUserId == userID
                                                               && c.CardNumber == request.SenderCardNumber,
                                                               [nameof(Card.Account)]);

                    if(senderCard is null)
                        return Result.Failure(CardError.NotFound);
                }

                if (senderAccount?.Status != AccountStatus.Active 
                    || 
                    senderCard?.Account.Status != AccountStatus.Active 
                    || 
                    senderCard?.Status != CardStatus.Active)
                 return Result.Failure(TransactionError.InvalidStatus);


                if (senderAccount?.Balance < request.Amount 
                    || senderCard?.Account.Balance < request.Amount)
                return Result.Failure(TransactionError.NoEnoughBalance);
            }

            if (request.Type == TransactionType.Transfer
            || request.Type == TransactionType.Deposit)
            {
                if (request.recieverAccount is not null)
                {
                    recieverAccount = await _unitOfWork.AccountRepository
                                       .FirstOrDefaultAsync(a => a.AppUserId == userID
                                                              && a.IBAN == request.recieverAccount);

                    if (recieverAccount is null)
                        return Result.Failure(AccountError.NotFound);
                }
                else
                {
                    recieverCard = await _unitOfWork.CardRepository
                                        .FirstOrDefaultAsync(c => c.Account.AppUserId == userID
                                                               && c.CardNumber == request.RecieverCardNumber);

                    if (recieverCard is null)
                        return Result.Failure(CardError.NotFound);
                }
            }

            //Card tarixin yoxla, Card-a PIN



        }
    }
}
