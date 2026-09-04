using CorePay.Application.Interfaces.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Interfaces.Services
{
    public interface ISystemValueGeneratorService
    {
        bool CheckIBAN(string iban);
        Task<string> GenerateIbanAsync();
        Task<string> GenerateCardNumberAsync();
        string GenerateCvnCode();
    }
}
