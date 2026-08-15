using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Auth.Register
{
    public class RegisterCommandValidation : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidation()
        {
            RuleFor(r => r.Name)
                .NotEmpty()
                .MaximumLength(60)
                .MinimumLength(3)
                .Must(n => n.All(n => char.IsLetter(n)))
                .WithMessage("Invalid Name syntax.");

            RuleFor(r => r.Surname)
              .NotEmpty()
              .MaximumLength(60)
              .MinimumLength(3)
              .Must(n => n.All(n => char.IsLetter(n)))
              .WithMessage("Invalid Surname syntax.");

            RuleFor(r => r.Username)
                .NotEmpty()
                .MaximumLength(256)
                .WithMessage("Invalid Username syntax.");

            RuleFor(r => r.Password)
                  .NotEmpty()
                  .MinimumLength(7)
                  .MaximumLength(256)
                  .Must(l => l.Any(l => char.IsUpper(l))
                          && l.Any(l => char.IsLower(l))
                          && l.Any(l => char.IsDigit(l)))
                  .WithMessage("Invalid Password syntax.");


            RuleFor(q => q.Email)
                  .NotEmpty()
                  .MinimumLength(4)
                  .MaximumLength(256);
                  //.Matches(@"^\w+([-+.']\\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")
                  //  .WithMessage("Invalid email address.");

            RuleFor(r => r.FIN)
                .NotEmpty()
                .Length(7)
                .Matches("^[A-Z0-9]{7}$")
                .WithMessage("FIN must contain exactly 7 uppercase letters or digits.");

            RuleFor(r => r.Birthdate)
                .NotEmpty()
                .Must(bd => bd.AddYears(18) <= DateOnly.FromDateTime(DateTime.Now));

            RuleFor(r => r.PhoneNumber)
                .NotEmpty()
                .Matches(@"^\+994(10|50|51|55|60|70|77|99)\d{7}$")
                .WithMessage("Invalid Azerbaijani phone number");

        }
    }
}
