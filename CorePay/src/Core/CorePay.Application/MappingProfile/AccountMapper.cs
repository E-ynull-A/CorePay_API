using AutoMapper;
using CorePay.Application.Features.Queries.Accounts.GetAll;
using CorePay.Application.Features.Queries.Accounts.GetById;
using CorePay.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.MappingProfile
{
    public class AccountMapper:Profile
    {
        public AccountMapper()
        {
            CreateMap<Account, GetAllAccountResponse>();
            CreateMap<Account, GetByIdAccountResponse>();
        }
    }
}
