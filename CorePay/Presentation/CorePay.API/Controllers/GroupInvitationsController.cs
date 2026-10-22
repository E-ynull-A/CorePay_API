using CorePay.API.Extentions;
using CorePay.Application.Common;
using CorePay.Application.Features.Commands.GroupInvitations.Accept;
using CorePay.Application.Features.Commands.GroupInvitations.Post;
using CorePay.Application.Features.Commands.GroupInvitations.Reject;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CorePay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupInvitationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GroupInvitationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("/GroupInvitation/Post")]

        public async Task<IActionResult> Post(PostGroupInvitationCommand command)
        {
            Result result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPut("/GroupInvitation/Accept")]

        public async Task<IActionResult> Accept(AcceptGroupInvitationCommand command)
        {
            Result result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPut("/GroupInvitation/Reject")]
        public async Task<IActionResult> Reject(RejectGroupInvitationCommand command)
        {
            Result result = await _mediator.Send(command);
            return result.ToActionResult();
        }
    }
}
