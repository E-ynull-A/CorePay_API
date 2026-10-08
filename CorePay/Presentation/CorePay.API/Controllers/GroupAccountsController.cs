using CorePay.API.Extentions;
using CorePay.Application.Common;
using CorePay.Application.Features.Commands.GroupAccounts.Post;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CorePay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupAccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GroupAccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "User")]
        [HttpPost("/GroupAccount/Post")]
        public async Task<IActionResult> Post([FromForm]PostGroupAccountCommand command)
        {
            Result result = await _mediator.Send(command);

            return result.ToActionResult();
        }
    }
}
