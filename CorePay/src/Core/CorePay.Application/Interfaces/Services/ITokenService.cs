using CorePay.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(AppUser appUser,
                                   string[] roles);

        string GenerateRefreshToken();
    }
}
