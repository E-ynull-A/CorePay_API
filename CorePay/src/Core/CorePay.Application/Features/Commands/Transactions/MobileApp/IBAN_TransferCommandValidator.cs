using CorePay.Application.Interfaces.Services;
using FluentValidation;

namespace CorePay.Application.Features.Commands.Transactions.MobileApp
{
    public class IBAN_TransferCommandValidator:AbstractValidator<IBAN_TransferCommand>
    {
        private readonly ISystemValueGeneratorService _generatorService;

        public IBAN_TransferCommandValidator(ISystemValueGeneratorService generatorService)
        {
            _generatorService = generatorService;

            RuleFor(tc => tc.RecieverAccountIBAN)
                .Must(ib => _generatorService.CheckIBAN(ib))
                .WithMessage("Invalid IBAN Input!");
               
        }
    }
}
