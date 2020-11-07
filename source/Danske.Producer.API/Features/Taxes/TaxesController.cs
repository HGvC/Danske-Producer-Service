using Danske.Producer.API.Extensions;
using Danske.Producer.Application.Taxes.Commands;
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
        private readonly IImportTaxHandler _importTaxHandler;

        public TaxesController(IMediator mediator, IImportTaxHandler importTaxHandler)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _importTaxHandler = importTaxHandler ?? throw new ArgumentNullException(nameof(importTaxHandler));
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async ValueTask<IActionResult> ImportTaxes(IFormFile file)
        {
            if (!ModelState.IsValid)
                return BadRequest("Wrong parameter format");

            try
            {
                var stream = file.OpenReadStream();
                await _importTaxHandler.ImportTaxes(stream);

                return Ok();
            }
            catch (ApplicationException applicationException)
            {
                return Ok(applicationException.Message);
            }
            catch (Exception)
            {
                return Problem("Unhandled request");
            }
        }
    }
}