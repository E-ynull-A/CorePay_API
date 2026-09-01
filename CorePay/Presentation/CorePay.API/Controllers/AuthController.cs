using CorePay.API.Extentions;
using CorePay.Application.Common;
using CorePay.Application.Features.Commands.Auth.EmailConfirm.Confirm;
using CorePay.Application.Features.Commands.Auth.EmailConfirm.Send;
using CorePay.Application.Features.Commands.Auth.Login;
using CorePay.Application.Features.Commands.Auth.Logout;
using CorePay.Application.Features.Commands.Auth.OtpConfirm.Confirm;
using CorePay.Application.Features.Commands.Auth.OtpConfirm.Send;
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
        public async Task<IActionResult> Login([FromForm] LoginCommand command)
        {
            Result<LoginCommandResponce> result = await _mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpDelete("/Logout")]
        public async Task<IActionResult> Logout(LogoutCommand command)
        {
            Result result = await _mediator.Send(command);

            return result.ToActionResult(204);
        }

        [HttpPost("/Refresh")]
        public async Task<IActionResult> Refresh([FromForm] RefreshCommand command)
        {
            Result<RefreshCommandResponse> response = await _mediator.Send(command);

            return response.ToActionResult();
        }

        [HttpPatch("/EmailConfirm")]
        public async Task<IActionResult> Confirm([FromForm] EmailConfirmCommand command)
        {
            Result response = await _mediator.Send(command);
            return response.ToActionResult(200);
        }

        [HttpPost("/Email/ResendConfirmCode")]
        public async Task<IActionResult> Resend([FromForm] ConfirmEmailResendCommand command)
        {
            Result response = await _mediator.Send(command);
            return response.ToActionResult(200);
        }

        [HttpPost("/OtpEmail/Send")]
        public async Task<IActionResult> Send([FromBody]SendOptConfirmCommand command)
        {
            Result result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPost("/OtpEmail/Confirm")]
        public async Task<IActionResult> Confirm([FromBody]ConfirmOptCommand command)
        {
            Result result = await _mediator.Send(command);
            return result.ToActionResult();
        }
    }
}
