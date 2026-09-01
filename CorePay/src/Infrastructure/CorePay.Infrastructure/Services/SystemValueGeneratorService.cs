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
        private readonly ICardRepository _cardRepository;

        public SystemValueGeneratorService(IConfiguration config,
                                            IAccountRepository accountRepository,
                                            ICardRepository cardRepository)
        {
            _config = config;
            _accountRepository = accountRepository;
            _cardRepository = cardRepository;
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

            string ibanForCheckDigit = string.Concat(bankCode, accountCode, countryCode, "00");

            return string.Concat(_config["Bank:Country"],
                                _getCheckDigit(ibanForCheckDigit),
                                _config["Bank:Code"], accountCode);
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


        public async Task<string> GenerateCardNumberAsync()
        {
            //Bank:VIM:4563 2844

            string code = string.Concat(_config["Bank:VIM"]
                                        ,RandomNumberGenerator.GetInt32(1000000,9999999).ToString());
            
            
            int numb = 0;
            int sum = 0;
            int checkDigit = 0;

            do
            {
                for (int i = 0; i < code.Length; i++)
                {
                    numb = code[i] - '0';

                    if (i % 2 == 0)
                        sum += (numb * 2 > 9)
                                    ? (numb * 2) - 9
                                    : numb * 2;

                    else
                        sum += numb;
                }

                checkDigit = (10 - (sum % 10)) % 10;
            }
            while (await _cardRepository.AnyAsync(c => c.CardNumber.StartsWith(code)));

                return string.Concat(code,checkDigit);
        }
        public string GenerateCvnCode() =>
            RandomNumberGenerator.GetInt32(100, 999).ToString("D3");
        
    }


}














