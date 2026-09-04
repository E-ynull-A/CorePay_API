using CorePay.Application.Common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Entities;
using CorePay.Domain.Exceptions;
using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors;
using MediatR;

namespace CorePay.Application.Features.Commands.Transactions.MobileApp
{
    public class IBAN_TransferCommadHandler : IRequestHandler<IBAN_TransferCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly IRedisCasheService _casheService;
        private readonly IOtpService _otpService;

        public IBAN_TransferCommadHandler(IUnitOfWork unitOfWork,
                                          ICurrentUserService currentUser,
                                          IRedisCasheService casheService,
                                          IOtpService otpService)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _casheService = casheService;
            _otpService = otpService;
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

            Account? reciever = await _unitOfWork.AccountRepository
                                                     .FirstOrDefaultAsync(a => a.IBAN == request.RecieverAccountIBAN);

            if (reciever is null)
                return Result.Failure(AccountError.NotFound);

            if (sender.Id == reciever.Id)
                return Result.Failure(TransactionError.SelfTransfer);

            if (reciever.Status != AccountStatus.Active)
                return Result.Failure(TransactionError.InvalidStatus);

            if (request.Amount > 100)
            {
                if (!await _casheService.AnyAsync($"otp-confirmed:{OtpPurpose.HighAmountTransfer}:{userId}"))
                {
                    string? email = _currentUser.GetUserEmail();

                    if (email is null)
                        return Result.Failure(AuthError.NotFound);

                    Result result = await _otpService
                                         .SendConfirmOtpAsync(email,
                                             OtpPurpose.HighAmountTransfer, 3.5);
                    if (!result.IsSuccess)
                        return result;

                    await _casheService.SetAsync($"pending-amount:{userId}:{sender.Id}",
                                                  request.Amount.ToString(),TimeSpan.FromMinutes(3.5));

                    return Result.Failure(TransactionError.OtpRequired);
                }
                else
                {
                    await _casheService.DeleteAsync($"otp-confirmed:{OtpPurpose.HighAmountTransfer}:{userId}");
                    await _casheService.DeleteAsync($"pending-amount:{userId}:{sender.Id}");
                }
            }

            sender.DecreaseBalance(reciever.Balance);
            reciever.IncreaseBalance(reciever.Balance);

            _unitOfWork.AccountRepository.Update(reciever);
            _unitOfWork.AccountRepository.Update(sender);

            var dbTransaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                Transaction senderTransaction = new Transaction(-request.Amount,
                                                                TransactionType.Transfer,
                                                                sender.Id);

                Transaction recieverTransaction = new Transaction(request.Amount,
                                                                  TransactionType.Transfer,
                                                                  reciever.Id);

                Transfer transfer = new Transfer(sender.Id,reciever.Id);

                transfer.Transactions.Add(senderTransaction);
                transfer.Transactions.Add(recieverTransaction);

                _unitOfWork.TransactionRepository.Add(senderTransaction);
                _unitOfWork.TransactionRepository.Add(recieverTransaction);

                _unitOfWork.TransferRepository.Add(transfer);

                await _unitOfWork.SaveChangeAsync();
                await dbTransaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await dbTransaction.RollbackAsync(cancellationToken);

                throw new TransactionException("Transfer process was Failed!",ex);
            }
        }
    }
}
//OTP SİLİNMƏSİNƏ NAZARAT TƏLƏB OLUNUR!!!