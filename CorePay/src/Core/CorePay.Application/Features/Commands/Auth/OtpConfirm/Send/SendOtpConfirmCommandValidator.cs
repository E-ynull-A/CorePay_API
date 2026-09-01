using CorePay.Domain.Utilities.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Auth.OtpConfirm.Send
{
    public class SendOtpConfirmCommandValidator:AbstractValidator<SendOptConfirmCommand>
    {
        public SendOtpConfirmCommandValidator()
        {
            RuleFor(so => so.Purpose)
                .NotEmpty()
                .IsInEnum();


        }
    }
}
