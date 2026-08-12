using CorePay.Application.Interfaces.Repositories;
using CorePay.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Infrastructure.Services
{
    public class SystemValueGeneratorService : ISystemValueGeneratorService
    {
        private readonly IConfiguration _config;
        private readonly IAccountRepository _accountRepository;

        public SystemValueGeneratorService(IConfiguration config,
                                            IAccountRepository accountRepository)
        {
            _config = config;
            _accountRepository = accountRepository;
        }
        public async Task<string> GenerateIbanAsync()
        {
            string bankCode = _convertToNumbers(_config["Bank:Code"]);
            string countryCode = _convertToNumbers(_config["Bank:Country"]);

            string? accountCode = string.Empty;

            do
            {
                accountCode = string.Concat(Enumerable.Range(0, 20)
               .Select(_ => RandomNumberGenerator.GetInt32(0, 10)));
            }
            while (await _accountRepository.AnyAsync(a => a.IBAN.EndsWith(accountCode)));

            string ibanForCheckDigit = string.Concat(bankCode,accountCode,countryCode,"00");

            return string.Concat(_config["Bank:Country"],
                                _getCheckDigit(ibanForCheckDigit),
                                _config["Bank:Code"],accountCode);
        }

        private int _getCheckDigit(string letters)
        {
            int reminder = 0;

            foreach (char letter in letters)
            {
                int numb = letter - '0';

                reminder = (reminder * 10 + numb) % 97;
            }

            return 98 - reminder;
        }

        private string _convertToNumbers(string letters)
        {
            return string
                .Concat(letters.Select(l => (l - 'A' + 10)
                    .ToString()));
        }
    }
}
