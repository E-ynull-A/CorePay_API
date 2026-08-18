using CorePay.API.Extentions;
using CorePay.Application.Common;
using CorePay.Application.Features.Commands.Accounts.Close;
using CorePay.Application.Features.Commands.Accounts.Post;
using CorePay.Application.Features.Commands.Accounts.StatusToggle.User;
using CorePay.Application.Features.Queries.Accounts.GetAll;
using CorePay.Application.Features.Queries.Accounts.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpGet("/Account/GetAll")]
        public async Task<IActionResult> Get([FromQuery]GetAllAccountQuery query)
        {
            Result<ICollection<GetAllAccountResponse>> response = await _mediator.Send(query);

            return response.ToActionResult();
        }

        [Authorize]
        [HttpPost("/Account/Post")]
        public async Task<IActionResult> Post([FromForm] PostAccountCommand command)
        {
            Result result = await _mediator.Send(command);

            return result.ToActionResult(201);
        }

        [Authorize]
        [HttpPut("/Account/User/ToggleStatus/{AccountId}")]
        public async Task<IActionResult> Put([FromRoute] ToggleStatusByUserCommand command)
        {
            Result result = await _mediator.Send(command);

            return result.ToActionResult(204);
        }

        [Authorize]
        [HttpGet("/Account/Get/{Id}")]

        public async Task<IActionResult> Get([FromRoute]GetByIdAccountQuery query)
        {
            Result<GetByIdAccountResponse> result = await _mediator.Send(query);

            return result.ToActionResult();
        }

        [Authorize]
        [HttpPut("/Account/Close/{Id}")]
        public async Task<IActionResult> Close([FromRoute] CloseAccountCommand query)
        {
            Result result = await _mediator.Send(query);

            return result.ToActionResult(204);
        }


    }
}
