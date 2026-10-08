using CorePay.Domain.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Queries.Transactions.GetAll
{
    public record GetAllTransactionQueryResponse(Guid Id,
                                                 decimal Amount,
                                                 TransactionType Type,
                                                 DateTimeOffset CreatedAt,
                                                 Currency Currency);
   
}
