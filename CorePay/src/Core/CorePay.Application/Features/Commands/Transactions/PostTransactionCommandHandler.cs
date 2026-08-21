using CorePay.Application.Common;
using CorePay.Application.Interfaces.Repositories;
using CorePay.Application.Interfaces.Services;
using CorePay.Domain.Entities;
using CorePay.Domain.Utilities.Errors;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
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

            Account? sender = await _unitOfWork.AccountRepository
                                        .FirstOrDefaultAsync(a => a.AppUserId == userID
                                                               && a.IBAN == request.SenderIBAN);

            if (sender is not null)
                return Result.Failure(AccountError.NotFound);

            Account? reciever = await _unitOfWork.AccountRepository
                                        .FirstOrDefaultAsync(a => a.IBAN == request.RecieverIBAN);

            if (reciever is not null)
                return Result.Failure(AccountError.NotFound);

            if(request.CardNumber is not null)
            {
                if (!await _unitOfWork.CardRepository.AnyAsync(c => c.CardNumber == request.CardNumber
                                                     && c.Account.IBAN == request.SenderIBAN))
                    return Result.Failure(CardError.NotFound);
            }  

            if(request.RecieverIBAN == request.SenderIBAN)
                return Result.Failure(AccountError.NotFound);
      
        }
    }
}
