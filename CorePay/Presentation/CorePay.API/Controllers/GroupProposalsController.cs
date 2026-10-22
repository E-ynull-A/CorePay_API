using CorePay.API.Extentions;
using CorePay.Application.Common;
using CorePay.Application.Features.Commands.GroupProposals.Post;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CorePay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupProposalsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GroupProposalsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("GroupProposal/Post")]

        public async Task<IActionResult> Post(PostGroupProposalCommand command)
        {
            Result result = await _mediator.Send(command);
            return result.ToActionResult();
        }
    }
}
