using CorePay.Application.Common;
using CorePay.Application.Features.Commands.Transactions.MobileApp.common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Entities;
using CorePay.Domain.Exceptions;
using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;


namespace CorePay.Application.Features.Commands.Transactions.MobileApp.CardToCard
{
    public class CardTransferCommandHandler : IRequestHandler<CardTransferCommand, Result>
    {
        private readonly ICardRepository _cardRepository;
        private readonly IOtpService _otpService;
        private readonly ITransferService _transferService;
        private readonly ICurrentUserService _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public CardTransferCommandHandler(ICardRepository cardRepository,
                                          IOtpService otpService,
                                          ITransferService transferService,
                                          ICurrentUserService currentUser,
                                          IUnitOfWork unitOfWork)
        {
            _cardRepository = cardRepository;
            _otpService = otpService;
            _transferService = transferService;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(CardTransferCommand request, CancellationToken cancellationToken)
        {
            Guid userId = _currentUser.GetUserId();

            Card? sender = await _cardRepository
                                .FirstOrDefaultAsync(c => c.Id == request.SenderCardId
                                                       && c.Account.AppUserId == userId,
                                                                [nameof(Card.Account)]);

            if (sender is null)
                return Result.Failure(CardError.NotFound);

            Card? receiver = await _cardRepository
                                .FirstOrDefaultAsync(c => c.CardNumber == request.ReceiverCardNumber,
                                                     [nameof(Card.Account)]);

            if (receiver is null)
                return Result.Failure(CardError.NotFound);

            if (sender.Id == receiver.Id)
                return Result.Failure(TransactionError.SelfTransfer);

            if (sender.Account.Status != AccountStatus.Active
              || receiver.Account.Status != AccountStatus.Active)
                return Result.Failure(TransactionError.InvalidStatus);

            if (sender.Status != CardStatus.Active
              || receiver.Status != CardStatus.Active)
                return Result.Failure(TransactionError.InvalidStatus);



            if (sender.Account.Currency != receiver.Account.Currency)
                return Result.Failure(TransactionError.DifferentCurrencyTransfer);

            if (sender.Account.Balance < request.Amount)
                return Result.Failure(TransactionError.NoEnoughBalance);

            if (request.Amount > 100)
            {
                PendingTransferContext transferContext =
                                        new(sender.AccountId,
                                            receiver.AccountId,
                                            request.Amount);

                Result result = await _transferService
                                             .HighAmountTransferAsync(userId, request.SessionId,
                                                                         transferContext);

                if (!result.IsSuccess)
                    return result;

                Result checkResult = await _transferService
                                                .CheckTransferContextAsync(userId, request.SessionId,
                                                                            transferContext);

                if (!checkResult.IsSuccess)
                    return checkResult;
            }

            sender.Account.DecreaseBalance(request.Amount);
            receiver.Account.IncreaseBalance(request.Amount);

            _unitOfWork.AccountRepository.Update(sender.Account);
            _unitOfWork.AccountRepository.Update(receiver.Account);

            IDbContextTransaction transactionDb = await _unitOfWork
                                        .BeginTransactionAsync(cancellationToken);

            try
            {
                Transaction send = new Transaction(request.Amount,
                                                   TransactionType.Transfer,
                                                   sender.AccountId,
                                                   sender.Id);

                Transaction receive = new Transaction(request.Amount,
                                                      TransactionType.Transfer,
                                                      receiver.Account.Id,
                                                      receiver.Id);

                Transfer cardTransfer = new Transfer(sender.AccountId, receiver.AccountId,
                                                     request.Amount,
                                                     sender.Id, receiver.Id);

                cardTransfer.Transactions.Add(send);
                cardTransfer.Transactions.Add(receive);

                send.AsignTransfer(cardTransfer);
                receive.AsignTransfer(cardTransfer);

                send.Validate();
                receive.Validate();

                _unitOfWork.TransferRepository.Add(cardTransfer);

                _unitOfWork.TransactionRepository.Add(send);
                _unitOfWork.TransactionRepository.Add(receive);

               

                await _unitOfWork.SaveChangeAsync();
                await transactionDb.CommitAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                await transactionDb.RollbackAsync(cancellationToken);
                throw new TransactionException("Transfer process was Failed!", ex);
            }   
        }
    }
}
