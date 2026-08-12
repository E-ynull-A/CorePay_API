using CorePay.API.Extentions;
using CorePay.Application.Common;
using CorePay.Application.Features.Commands.Auth.Login;
using CorePay.Application.Features.Commands.Auth.Refresh;
using CorePay.Application.Features.Commands.Auth.Register;
using MediatR;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CorePay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("/Register")]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            Result result = await _mediator.Send(command);
            return result.ToActionResult(201);
        }

        [HttpPost("/Login")]
        public async Task<IActionResult> Login([FromForm]LoginCommand command)
        {
            Result<LoginCommandResponce> result = await _mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpPost("/Refresh")]
        public async Task<IActionResult> Refresh([FromForm]RefreshCommand command)
        {
           Result<RefreshCommandResponse> response = await _mediator.Send(command);

            return response.ToActionResult();
        }
    }
}
