using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sol_Demo.Features;

namespace Sol_Demo.Controllers
{
    [Route("api/sales")]
    [Produces("application/json")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly IMediator? mediator = null;

        public SalesController(IMediator? mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("sales-order")]
        public async Task<IActionResult> GetSalesOrderData([FromQuery] GetSalesOrderFilterQuery getSalesOrderFilterQuery, CancellationToken cancellationToken)
        {
            var resultSet = await mediator!.Send(getSalesOrderFilterQuery, cancellationToken);

            if (!resultSet.Any())
                return NotFound();

            return Ok(resultSet);
        }
    }
}