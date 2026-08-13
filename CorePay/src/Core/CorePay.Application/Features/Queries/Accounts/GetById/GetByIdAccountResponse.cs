using CorePay.Domain.Entities;
using CorePay.Domain.Utilities.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Queries.Accounts.GetById
{
    public record GetByIdAccountResponse(

        Guid Id,
        Currency Currency,
        string IBAN,
        decimal Balance,
        AccountStatus Status,
        DateTimeOffset CreatedAt);
   
}
