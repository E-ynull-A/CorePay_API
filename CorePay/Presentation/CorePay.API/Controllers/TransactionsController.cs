using CorePay.API.Extentions;
using CorePay.Application.Common;
using CorePay.Application.Features.Commands.Transactions.ATM.Auth;
using CorePay.Application.Features.Commands.Transactions.ATM.Withdraw;
using CorePay.Application.Features.Commands.Transactions.Deposit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CorePay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("/atm/authenticate")]

        public async Task<IActionResult> Authenticate([FromForm] AtmAuthenticationCommand command)
        {
            Result<AtmAuthenticationCommandResponse> result =
                                          await _mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpPost("/atm/withdraw")]
        public async Task<IActionResult> Withdraw(WithdrawCommand command)
        {
            Result result = await _mediator.Send(command);

            return result.ToActionResult(201);
        }

        [HttpPost("/terminal/deposit")]
        public async Task<IActionResult> Deposit([FromForm]DepositCommand command)
        {
            Result result = await _mediator.Send(command);

            return result.ToActionResult(201);
        }
    }
}
