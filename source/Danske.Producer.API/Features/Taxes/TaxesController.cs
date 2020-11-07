using Danske.Producer.API.Extensions;
using Danske.Producer.Application.Taxes.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Danske.Producer.API.Features.Taxes
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class TaxesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaxesController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GetTaxResponse>>> GetAsync([FromQuery] GetTaxRequest request)
        {
            var query = new GetTax(request.Municipality, request.Date);

            var result = await _mediator.Send(query);

            var response = result.Map(s => s.ToResponse());

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateTaxAsync([FromBody] UpdateTaxRequest request)
        {
            var command = request.ToCommand();

            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }
    }
}