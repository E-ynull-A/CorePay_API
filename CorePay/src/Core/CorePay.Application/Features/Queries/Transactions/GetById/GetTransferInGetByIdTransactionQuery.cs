using CorePay.Domain.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Queries.Transactions.GetById
{
    public record GetTransferInGetByIdTransactionQuery(string ReceiverFullName,
                                                       string MaskedDestination,
                                                       TransferType Type);
    
}
