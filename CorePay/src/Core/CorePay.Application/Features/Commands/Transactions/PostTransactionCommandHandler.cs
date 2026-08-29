using CorePay.Application.Common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Entities;
using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Transactions
{
    //public class PostTransactionCommandHandler : IRequestHandler<PostTransactionCommand, Result>
    //{
    //    private readonly IUnitOfWork _unitOfWork;
    //    private readonly ICurrentUserService _currentUser;
    //    private readonly IRedisCasheService _casheService;

    //    public PostTransactionCommandHandler(IUnitOfWork unitOfWork,
    //                                         ICurrentUserService currentUser,
    //                                         IRedisCasheService casheService)
    //    {
    //        _unitOfWork = unitOfWork;
    //        _currentUser = currentUser;
    //        _casheService = casheService;
    //    }
    //    public async Task<Result> Handle(PostTransactionCommand request, CancellationToken cancellationToken)
    //    {
    //        Guid userID = _currentUser.GetUserId();

    //        Account? SenderIBAN = default;
    //        Account? RecieverIBAN = default;

    //        Card? senderCard = default;
    //        Card? recieverCard = default;

    //        if (request.Type == TransactionType.Transfer
    //            || request.Type == TransactionType.Withdraw)
    //        {

    //            Expression<Func<Card, bool>> func = request.Type == TransactionType.Transfer

    //                ? c => c.Account.AppUserId == userID
    //                  && c.CardNumber == request.SenderCardNumber

    //                : c => c.CardNumber == request.SenderCardNumber;


    //            senderCard = await _unitOfWork.CardRepository
    //                                .FirstOrDefaultAsync(func,[nameof(Card.Account)]);

    //            if (senderCard is null)
    //                return Result.Failure(CardError.NotFound);

    //            if (senderCard.ExpireDate < DateOnly.FromDateTime(DateTime.Now))
    //                return Result.Failure(CardError.Expired);

    //            if (senderCard?.Account.Status != AccountStatus.Active
    //                ||
    //                senderCard?.Status != CardStatus.Active)
    //                return Result.Failure(TransactionError.InvalidStatus);

    //            if (senderCard?.Account.Balance < request.Amount)
    //                return Result.Failure(TransactionError.NoEnoughBalance);

    //            if (request.Type == TransactionType.Withdraw)
    //            {
    //                PasswordHasher<object> hasher = new PasswordHasher<object>();

    //                var result = hasher.VerifyHashedPassword(null!, senderCard.PinHash, request.PIN);

    //                if (result == PasswordVerificationResult.Failed)
    //                {
    //                    if (await _casheService.CountAsync($"card:verify-pin:attempts:{senderCard.Id}", TimeSpan.FromMinutes(15)) == 3)
    //                        return Result.Failure(TransactionError.TooManyAttempts);

    //                    return Result.Failure(TransactionError.WrongPIN);
    //                }
    //            }
    //            else
    //            {
                    
    //            }


    //        }

    //        if (request.Type == TransactionType.Transfer)
    //        {

    //            SenderIBAN = await _unitOfWork.AccountRepository
    //                               .FirstOrDefaultAsync(a => a.AppUserId == userID
    //                                                      && a.IBAN == request.SenderIBAN);

    //            if (SenderIBAN is null)
    //                return Result.Failure(AccountError.NotFound);

    //            if (SenderIBAN?.Status != AccountStatus.Active)
    //                return Result.Failure(TransactionError.InvalidStatus);

    //            if (SenderIBAN?.Balance < request.Amount)
    //                return Result.Failure(TransactionError.NoEnoughBalance);
    //        }

    //        if (request.Type == TransactionType.Transfer
    //        || request.Type == TransactionType.Deposit)
    //        {
    //            if (request.RecieverIBAN is not null)
    //            {
    //                RecieverIBAN = await _unitOfWork.AccountRepository
    //                                   .FirstOrDefaultAsync(a => a.IBAN == request.RecieverIBAN);

    //                if (RecieverIBAN is null)
    //                    return Result.Failure(AccountError.NotFound);
    //            }
    //            else
    //            {
    //                recieverCard = await _unitOfWork.CardRepository
    //                                    .FirstOrDefaultAsync(c => c.CardNumber == request.RecieverCardNumber
    //                                                        ,[nameof(Card.Account)]);

    //                if (recieverCard is null)
    //                    return Result.Failure(CardError.NotFound);

    //                if (recieverCard.ExpireDate < DateOnly.FromDateTime(DateTime.Now))
    //                    return Result.Failure(CardError.Expired);
    //            }

    //            if (RecieverIBAN?.Status != AccountStatus.Active
    //              ||
    //              recieverCard?.Account.Status != AccountStatus.Active
    //              ||
    //              recieverCard?.Status != CardStatus.Active)

    //                return Result.Failure(TransactionError.InvalidStatus);
    //        }

    //        var trasactionDb = _unitOfWork.BeginTransactionAsync();

    //        try
    //        {
    //            switch (request.Type)
    //            {
    //                case TransactionType.Withdraw:



    //                    break;
    //            }



    //        }
    //        catch (Exception ex)
    //        {

    //        }
    //    }
    //}
}
