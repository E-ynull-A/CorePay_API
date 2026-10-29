using CorePay.Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace CorePay.Application.Features.Commands.Transactions.MobileApp.IBAN
{
    public record IBAN_TransferCommand(Guid SenderAccountId,
                                      decimal Amount,
                                      string RecieverAccountIBAN,
                                      string? SessionId = null) : IRequest<Result>
    {
        [JsonIgnore]
        public string IdempotencyKey { get; init; } = string.Empty;
    }
}
    

