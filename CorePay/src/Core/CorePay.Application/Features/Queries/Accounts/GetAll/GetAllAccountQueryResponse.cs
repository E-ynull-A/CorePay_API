using CorePay.Domain.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Queries.Accounts.GetAll
{
    public record GetAllAccountQueryResponse(
        
        Guid Id,
        Currency Currency,
        string IBAN,
        decimal Balance,
        AccountStatus Status);
  
}
