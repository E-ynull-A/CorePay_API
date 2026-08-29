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
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Transactions.ATM.Auth
{
    public class AtmAuthenticationCommandHandler : IRequestHandler<AtmAuthenticationCommand, Result<AtmAuthenticationCommandResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisCasheService _casheService;

        public AtmAuthenticationCommandHandler(IUnitOfWork unitOfWork,
                                               IRedisCasheService casheService)
        {
            _unitOfWork = unitOfWork;
            _casheService = casheService;
        }
        public async Task<Result<AtmAuthenticationCommandResponse>> Handle(AtmAuthenticationCommand request, CancellationToken cancellationToken)
        {
            Card? card = await _unitOfWork.CardRepository
                .FirstOrDefaultAsync(c=>c.CardNumber == request.CardNumber,
                                    [nameof(Card.Account)]);

            if (card is null)
                return Result<AtmAuthenticationCommandResponse>.Failure(CardError.NotFound);

            if(card.IsExpired())
                return Result<AtmAuthenticationCommandResponse>.Failure(CardError.Expired);

            if (card.Status != CardStatus.Active)
                return Result<AtmAuthenticationCommandResponse>.Failure(TransactionError.InvalidStatus);

            if(card.Account.Status != AccountStatus.Active)
                return Result<AtmAuthenticationCommandResponse>.Failure(TransactionError.InvalidStatus);

            PasswordHasher<object> hasher = new PasswordHasher<object>();

            PasswordVerificationResult verificationResult = hasher
                                                    .VerifyHashedPassword(null!, card.PinHash, request.PIN);

            if (verificationResult == PasswordVerificationResult.Failed)
                return Result<AtmAuthenticationCommandResponse>.Failure(TransactionError.WrongPIN);

            string sessionId = Guid.NewGuid().ToString();

            await _casheService.SetAsync($"atm-sessionId:{sessionId}",
                                              card.Id,TimeSpan.FromMinutes(15));

            return Result<AtmAuthenticationCommandResponse>
                .Success(new AtmAuthenticationCommandResponse(sessionId));
        }
    }
}
