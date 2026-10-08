using CorePay.Domain.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Queries.Transactions.GetById
{
    public record GetByIdTransactionQueryResponse(Guid Id,
                                             Guid AccountId,
                                             decimal Amount,
                                             Currency Currency,
                                             TransactionType Type,
                                             DateTimeOffset CreatedAt,
                                             Guid? CardId = null,
                                             GetTransferInGetByIdTransactionQuery? Transfer = null);
   
}
