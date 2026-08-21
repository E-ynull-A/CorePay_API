using CorePay.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Queries.Cards.GetAll
{
    public record GetAllCardQuery(Guid AccountId):IRequest<Result<ICollection<GetAllCardResponse>>>;
    
}
