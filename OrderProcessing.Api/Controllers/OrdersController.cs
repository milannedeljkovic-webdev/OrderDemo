using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderProcessing.Application.Orders;

namespace OrderProcessing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ISender _mediator;

    public OrdersController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand command)
    {
        try
        {
            var id = await _mediator.Send(command);
            return Ok(new { OrderId = id, Message = "Uspešno kupljeno!" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Error = ex.Message }); // Npr. Nema robe
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }
}