using CorePay.Application.Common;
using CorePay.Domain.Utilities.Enums;
using MediatR;

namespace CorePay.Application.Features.Commands.Transactions
{
    public record PostTransactionCommand(
        decimal Amount,
        TransactionType Type,

        string? SenderIBAN,
        string? RecieverIBAN,

        string? SenderCardNumber,
        string? RecieverCardNumber,
        
        string PIN):IRequest<Result>;
    
   
}
