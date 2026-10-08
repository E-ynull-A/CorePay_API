using CorePay.API.Extentions;
using CorePay.Application.Common;
using CorePay.Application.Features.Commands.Transactions.ATM.Auth;
using CorePay.Application.Features.Commands.Transactions.ATM.Withdraw;
using CorePay.Application.Features.Commands.Transactions.Deposit;
using CorePay.Application.Features.Commands.Transactions.MobileApp.CardToCard;
using CorePay.Application.Features.Commands.Transactions.MobileApp.IBAN;
using CorePay.Application.Features.Queries.Accounts.GetById;
using CorePay.Application.Features.Queries.Transactions.GetAll;
using CorePay.Application.Features.Queries.Transactions.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("/history")]
        public async Task<IActionResult> Get([FromQuery]GetAllTransactionQuery query)
        {
          Result<ICollection<GetAllTransactionQueryResponse>>
                                result = await _mediator.Send(query);

          return result.ToActionResult();
        }

        [HttpGet("/history/detailed/{Id}")]
        public async Task<IActionResult> Get([FromRoute]GetByIdTransactionQuery query)
        {
            Result<GetByIdTransactionQueryResponse> result = 
                                            await _mediator.Send(query);

            return result.ToActionResult();
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

        [Authorize]
        [HttpPost("/mobileApp/transfer/IBAN")]
        public async Task<IActionResult> Transfer([FromForm]IBAN_TransferCommand command)
        {
            Result result = await _mediator.Send(command);

            return result.ToActionResult();
        }

        [Authorize]
        [HttpPost("/mobileApp/transfer/Card")]

        public async Task<IActionResult> Transfer([FromForm]CardTransferCommand command)
        {
            Result result = await _mediator.Send(command);

            return result.ToActionResult();
        }
    }
}
