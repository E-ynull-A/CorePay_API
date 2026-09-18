using CorePay.Application.Common;
using CorePay.Application.Features.Commands.Transactions.MobileApp.common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Entities;
using CorePay.Domain.Exceptions;
using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors;
using MediatR;

namespace CorePay.Application.Features.Commands.Transactions.MobileApp.IBAN
{
    public class IBAN_TransferCommadHandler : IRequestHandler<IBAN_TransferCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly IRedisCasheService _casheService;
        private readonly IOtpService _otpService;
        private readonly ITransferService _transferService;

        public IBAN_TransferCommadHandler(IUnitOfWork unitOfWork,
                                          ICurrentUserService currentUser,
                                          IRedisCasheService casheService,
                                          IOtpService otpService,
                                          ITransferService transferService)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _casheService = casheService;
            _otpService = otpService;
            _transferService = transferService;
        }
        public async Task<Result> Handle(IBAN_TransferCommand request, CancellationToken cancellationToken)
        {
            Guid userId = _currentUser.GetUserId();

            Account? sender = await _unitOfWork.AccountRepository
                                                    .FirstOrDefaultAsync(a => a.Id == request.SenderAccountId
                                                                           && a.AppUserId == userId);
            if (sender is null)
                return Result.Failure(AccountError.NotFound);

            if (sender.Status != AccountStatus.Active)
                return Result.Failure(TransactionError.InvalidStatus);

            if (sender.Balance < request.Amount)
                return Result.Failure(TransactionError.NoEnoughBalance);

            Account? receiver = await _unitOfWork.AccountRepository
                                                     .FirstOrDefaultAsync(a => a.IBAN == request.RecieverAccountIBAN);

            if (receiver is null)
                return Result.Failure(AccountError.NotFound);

            if (sender.Id == receiver.Id)
                return Result.Failure(TransactionError.SelfTransfer);

            if (sender.Currency != receiver.Currency)
                return Result.Failure(TransactionError.DifferentCurrencyTransfer);

            if (receiver.Status != AccountStatus.Active)
                return Result.Failure(TransactionError.InvalidStatus);

            if (request.Amount > 100)
            {
                PendingTransferContext transferContext =
                                new PendingTransferContext(sender.Id,
                                                           receiver.Id,
                                                           request.Amount);

                Result result = await _transferService
                                        .HighAmountTransferAsync(userId, request.SessionId, transferContext);

                if (!result.IsSuccess)
                    return result;

                Result checkResult = await _transferService
                                                 .CheckTransferContextAsync(userId, request.SessionId, transferContext);

                if(!checkResult.IsSuccess)
                    return checkResult;
            }

            sender.DecreaseBalance(request.Amount);
            receiver.IncreaseBalance(request.Amount);

            _unitOfWork.AccountRepository.Update(receiver);
            _unitOfWork.AccountRepository.Update(sender);

            var dbTransaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                Transaction senderTransaction = new Transaction(-request.Amount,
                                                                TransactionType.Transfer,
                                                                sender.Id);

                Transaction receiverTransaction = new Transaction(request.Amount,
                                                                  TransactionType.Transfer,
                                                                  receiver.Id);

                Transfer transfer = new Transfer(sender.Id,
                                                 receiver.Id,
                                                 request.Amount);

                transfer.Transactions.Add(senderTransaction);
                transfer.Transactions.Add(receiverTransaction);

                senderTransaction.AsignTransfer(transfer);
                receiverTransaction.AsignTransfer(transfer);

                senderTransaction.Validate();
                receiverTransaction.Validate();

                _unitOfWork.TransactionRepository.Add(senderTransaction);
                _unitOfWork.TransactionRepository.Add(receiverTransaction);

                _unitOfWork.TransferRepository.Add(transfer);

                await _unitOfWork.SaveChangeAsync();
                await dbTransaction.CommitAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                await dbTransaction.RollbackAsync(cancellationToken);

                throw new TransactionException("Transfer process was Failed!", ex);
            }


        }
    }
}
//OTP SİLİNMƏSİNƏ NAZARAT TƏLƏB OLUNUR!!!
//VALIDATE DIGƏR ƏMƏLİYYATLARDA TƏNZİMLƏ!!!