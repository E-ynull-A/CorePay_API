using CorePay.Application.Common;
using CorePay.Domain.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Interfaces.Services
{
    public interface IOtpService
    {
        Task<bool> IsTooManyAttempsAsync(string email,OtpPurpose purpose);
        Task<Result> SendConfirmEmailAsync(string toEmail
                                          ,OtpPurpose purpose
                                          ,int expireMinute);
    }
}
