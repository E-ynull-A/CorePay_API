using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Auth.OtpConfirm.Confirm
{
    public class ConfirmOtpCommandValidator:AbstractValidator<ConfirmOptCommand>
    {
        public ConfirmOtpCommandValidator()
        {
            RuleFor(co => co.Purpose)
                .NotEmpty()
                .IsInEnum();

            RuleFor(co => co.OtpCode)
                .NotEmpty()
                .Must(oc => oc.All(l => Char.IsDigit(l)))
                    .WithMessage("Invalid Code Input!");
        }
    }
}
