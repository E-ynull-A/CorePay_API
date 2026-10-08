using CorePay.Application.Common;
using MediatR;

namespace CorePay.Application.Features.Queries.Transactions.GetAll
{
    public record GetAllTransactionQuery(Guid AccountId,int page,int take)
                    :IRequest<Result<ICollection<GetAllTransactionQueryResponse>>>;
  
}
