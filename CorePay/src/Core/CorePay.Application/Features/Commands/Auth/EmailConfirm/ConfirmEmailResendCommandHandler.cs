using CorePay.Application.Common;
using CorePay.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.Auth.EmailConfirm
{
    public class ConfirmEmailResendCommandHandler : IRequestHandler<ConfirmEmailResendCommand, Result>
    {
        private readonly IEmailConfirmService _confirmService;

        public ConfirmEmailResendCommandHandler(IEmailConfirmService confirmService)
        {
            _confirmService = confirmService;
        }
        public async Task<Result> Handle(ConfirmEmailResendCommand request, CancellationToken cancellationToken)
        {
            Result emailResult = await _confirmService
                                            .SendConfirmEmailAsync(request.Email);

            if (!emailResult.IsSuccess)
                return emailResult;

            return Result.Success();
        }
    }
}
