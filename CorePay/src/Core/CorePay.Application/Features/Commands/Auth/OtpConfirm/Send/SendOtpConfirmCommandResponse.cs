using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Auth.OtpConfirm.Send
{
    public record SendOtpConfirmCommandResponse(string ChallengeId);
    
}
