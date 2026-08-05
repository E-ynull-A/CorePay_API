using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Queries.Token.Access
{
    public record PostAccessTokenQueryResponse(      
        string Username,
        DateTime Expires,
        string AccessToken);
    
}
