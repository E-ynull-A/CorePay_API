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

            Account? receiver = await _unitOfWork.AccountRepository
                                                     .FirstOrDefaultAsync(a => a.IBAN == request.RecieverAccountIBAN);

            if (receiver is null)
                return Result.Failure(AccountError.NotFound);

            if (sender.Id == receiver.Id)
                return Result.Failure(TransactionError.SelfTransfer);

            if (receiver.Status != AccountStatus.Active)
                return Result.Failure(TransactionError.InvalidStatus);

            if (request.Amount > 100)
            {
                if (!await _casheService.AnyAsync($"otp-confirmed:{OtpPurpose.HighAmountTransfer.ToString().ToLower()}:{userId}:{request.SessionId}"))
                {
                    string? email = _currentUser.GetUserEmail();

                    if (email is null)
                        return Result.Failure(AuthError.NotFound);

                    Result<string> result = await _otpService
                                                        .SendConfirmOtpAsync(email,OtpPurpose.HighAmountTransfer,3);

                    await _casheService.SetAsync($"pending-transfer:{userId.ToString()}:{result.Value}",
                                                  new PendingTransferContext(sender.Id,
                                                                             receiver.Id,
                                                                             request.Amount)
                                                  ,TimeSpan.FromMinutes(4));

                    return Result.Failure(TransactionError.OtpRequired with { Details = result.Value});
                }
                else
                {
                    PendingTransferContext? transferContext = await _casheService.GetAsync<PendingTransferContext>
                                                ($"pending-transfer:{userId.ToString()}:{request.SessionId}");

                    if (transferContext == default
                      || transferContext.Amount != request.Amount
                      || transferContext.SenderAccountId != sender.Id
                      || transferContext.ReceiverAccountId != receiver.Id)
                        return Result.Failure(TransactionError.InvalidOtpContext);

                    await _casheService.DeleteAsync($"otp-confirmed:{OtpPurpose.HighAmountTransfer}:{userId}");
                    await _casheService.DeleteAsync($"pending-amount:{userId}:{request.SessionId}");
                }

                //Qardaş send və confirm ayrı endpointlərdə
                //gedəcək bura qatma sadəcə sessionİd-ni tut göndər
                // O otp service-də yazdığın da rədd elə getsün xoroşo?!
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

                _unitOfWork.TransactionRepository.Add(senderTransaction);
                _unitOfWork.TransactionRepository.Add(receiverTransaction);

                _unitOfWork.TransferRepository.Add(transfer);

                senderTransaction.Validate();
                receiverTransaction.Validate();

                await _unitOfWork.SaveChangeAsync();
                await dbTransaction.CommitAsync(cancellationToken);

                return Result.Success();
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
//VALIDATE DIGƏR ƏMƏLİYYATLARDA TƏNZİMLƏ!!!