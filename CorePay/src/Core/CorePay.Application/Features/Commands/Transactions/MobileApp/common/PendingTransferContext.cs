using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Transactions.MobileApp.common
{
    public record PendingTransferContext(
        
        Guid SenderAccountId,
        Guid ReceiverAccountId,
        decimal Amount);
    
}
