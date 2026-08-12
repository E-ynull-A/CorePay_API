using CorePay.API.Extentions;
using CorePay.Application.Common;
using CorePay.Application.Features.Commands.Accounts.Post;
using CorePay.Application.Features.Commands.Accounts.StatusToggle.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CorePay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator) =>
            _mediator = mediator;

        [HttpPost("Account/Post")]
        public async Task<IActionResult> Post([FromForm] PostAccountCommand command)
        {
            Result result = await _mediator.Send(command);

            return result.ToActionResult(201);
        }

        [HttpPut("Account/User/ToggleStatus/{Id}")]
        public async Task<IActionResult> Put([FromRoute] ToggleStatusByUserCommand command)
        {
            Result result = await _mediator.Send(command);

            return result.ToActionResult(204);
        }


    }
}
