using CorePay.Application.Common;
using CorePay.Domain.Utilities.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Queries.Transactions.GetById
{
    public record GetByIdTransactionQuery(Guid Id):IRequest<Result<GetByIdTransactionQueryResponse>>;
  
}
