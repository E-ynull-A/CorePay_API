using CorePay.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Interfaces.Services
{
    public interface IEmailConfirmService
    {
        Task<bool> IsTooManyAttempsAsync(string email);
        Task<Result> SendConfirmEmailAsync(string toEmail);
    }
}
