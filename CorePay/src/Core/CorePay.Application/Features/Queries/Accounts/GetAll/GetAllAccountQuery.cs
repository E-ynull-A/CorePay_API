using CorePay.Application.Common;
using CorePay.Domain.Entities;
using MediatR;
using System;
using System.Linq.Expressions;


namespace CorePay.Application.Features.Queries.Accounts.GetAll
{
    public record GetAllAccountQuery(
        int Take,
        int Page):IRequest<Result<ICollection<GetAllAccountQueryResponse>>>;
    
}
