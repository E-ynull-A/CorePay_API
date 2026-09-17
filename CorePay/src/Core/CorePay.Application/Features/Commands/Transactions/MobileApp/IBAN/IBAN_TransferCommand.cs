using CorePay.Application.Common;
using MediatR;

namespace CorePay.Application.Features.Commands.Transactions.MobileApp.IBAN
{
    public record IBAN_TransferCommand(Guid SenderAccountId,
                                      decimal Amount,
                                      string RecieverAccountIBAN,
                                      string? SessionId = null):IRequest<Result>;
    
}
