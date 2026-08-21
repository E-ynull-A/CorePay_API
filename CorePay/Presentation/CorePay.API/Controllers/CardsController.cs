using CorePay.API.Extentions;
using CorePay.Application.Common;
using CorePay.Application.Features.Commands.Cards.Lost;
using CorePay.Application.Features.Commands.Cards.Post;
using CorePay.Application.Features.Commands.Cards.Remove;
using CorePay.Application.Features.Queries.Cards.GetAll;
using CorePay.Application.Features.Queries.Cards.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CorePay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CardsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [Authorize]
        [HttpPost("/Card/Post/{AccountId}")]
        public async Task<IActionResult> Post([FromRoute] PostCardCommand command)
        {
            Result result = await _mediator.Send(command);

            return result.ToActionResult(201);
        }

        [Authorize]
        [HttpPatch("/Card/Lock/{Id}")]
        public async Task<IActionResult> Lost([FromRoute] LockCardCommand command)
        {
            Result result = await _mediator.Send(command);

            return result.ToActionResult(204);
        }

        [Authorize]
        [HttpDelete("/Card/SoftDelete/{Id}")]

        public async Task<IActionResult> Remove([FromRoute] RemoveCardCommand command)
        {
            Result result = await _mediator.Send(command);

            return result.ToActionResult(204);
        }


        [Authorize]
        [HttpGet("/Card/GetAll")]

        public async Task<IActionResult> Get([FromQuery] GetAllCardQuery query)
        {
            Result<ICollection<GetAllCardResponse>> response =
                                                    await _mediator.Send(query);

            return response.ToActionResult();
        }

        [Authorize]
        [HttpGet("/Card/GetById")]

        public async Task<IActionResult> Get([FromQuery]GetByIdCardQuery query)
        {
            Result<GetByIdCardResponse> response = 
                                    await _mediator.Send(query);

            return response.ToActionResult();
        }
    }
}
