using CorePay.Application.Common;
using MediatR;

namespace CorePay.Application.Features.Commands.Transactions.MobileApp
{
    public record IBAN_TransferCommand(Guid SenderAccountId,
                                      decimal Amount,
                                      string RecieverAccountIBAN):IRequest<Result>;
    
}
