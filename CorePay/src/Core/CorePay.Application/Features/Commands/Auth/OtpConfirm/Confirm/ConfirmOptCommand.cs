using CorePay.Application.Common;
using CorePay.Domain.Utilities.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Auth.OtpConfirm.Confirm
{
    public record ConfirmOptCommand(string OtpCode,
                                    CriticalOtpPurpose Purpose):IRequest<Result>;
    
}
