using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Application.Services;
using Application.DTOs;
using System.Threading.Tasks;
// Removed unused using directive for EntityFrameworkCore.Storage

namespace Presentation.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class OrdersController : ControllerBase
{
    private readonly ProductService _service;

    public OrdersController(ProductService service)
    {
        _service = service;
    }

    [HttpPost("place")]
    public async Task<ActionResult> PlaceOrder([FromBody] PlaceOrderDto order)
    {
        var success = await _service.PlaceOrderAsync(order);
        if (!success) return BadRequest("Insufficient stock for one or more products.");
        return Ok("Order placed successfully.");
    }
}