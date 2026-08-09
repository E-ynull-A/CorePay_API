using AutoMapper;
using CorePay.Application.Features.Commands.Auth.Register;
using CorePay.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.MappingProfile
{
    public class AuthProfile:Profile
    {
        public AuthProfile()
        {
            CreateMap<RegisterCommand, AppUser>();
        }
    }
}
