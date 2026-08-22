using CorePay.Application.Common;
using CorePay.Domain.Utilities.Enums;
using MediatR;

namespace CorePay.Application.Features.Commands.Transactions
{
    public record PostTransactionCommand(
        decimal Amount,
        TransactionType Type,

        string? senderAccount,
        string? recieverAccount,

        string? SenderCardNumber,
        string? RecieverCardNumber):IRequest<Result>;
    
   
}
