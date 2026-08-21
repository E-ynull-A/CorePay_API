using AutoMapper;
using CorePay.Application.Features.Queries.Accounts.GetAll;
using CorePay.Application.Features.Queries.Cards.GetAll;
using CorePay.Application.Features.Queries.Cards.GetById;
using CorePay.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.MappingProfile
{
    public class CardMapper:Profile
    {
        public CardMapper()
        {
            CreateMap<Card, GetAllCardResponse>();
            CreateMap<Card, GetByIdCardResponse>();
        }
    }
}
