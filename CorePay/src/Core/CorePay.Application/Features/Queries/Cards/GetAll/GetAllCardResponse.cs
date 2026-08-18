using CorePay.Domain.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Queries.Cards.GetAll
{
    public record GetAllCardResponse(Guid Id,
                                     string CardNumber,
                                     CardStatus Status);
   
}
