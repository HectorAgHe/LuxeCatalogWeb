using FluentValidation;
using LuxeCatalog.Business.DTOs.Orders;
using LuxeCatalog.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LuxeCatalog.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IValidator<OrderRequest> _orderValidator;
    private readonly IValidator<UpdateOrderStatusRequest> _statusValidator;

    public OrdersController(
        IOrderService orderService,
        IValidator<OrderRequest> orderValidator,
        IValidator<UpdateOrderStatusRequest> statusValidator)
    {
        _orderService = orderService;
        _orderValidator = orderValidator;
        _statusValidator = statusValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _orderService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("by-status")]
    public async Task<IActionResult> GetByStatus([FromQuery] string status)
    {
        var result = await _orderService.GetByStatusAsync(status);
        return Ok(result);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        var result = await _orderService.GetByUserAsync(userId);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _orderService.GetByIdAsync(id);
        if (result is null)
            return NotFound(new { message = "Pedido no encontrado." });

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderRequest request)
    {
        var validation = await _orderValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        var result = await _orderService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusRequest request)
    {
        var validation = await _statusValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        var success = await _orderService.UpdateStatusAsync(id, request);
        if (!success)
            return BadRequest(new { message = "Pedido no encontrado o estado inválido." });

        return Ok(new { message = "Estado actualizado correctamente." });
    }
}