using CorePay.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Queries.Token.Access
{
    public record PostAccessTokenQuery(       
        AppUser User,
        int Minutes,
        string[] Roles):IRequest<PostAccessTokenQueryResponse>;
   
}
