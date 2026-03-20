using LuxeCatalog.Business.DTOs.Orders;
using LuxeCatalog.Business.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LuxeCatalog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET api/orders
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _orderService.GetAllAsync();
            return Ok(result);
        }

        // GET api/orders?status=Pendiente
        [HttpGet("by-status")]
        public async Task<IActionResult> GetByStatus([FromQuery] string status)
        {
            var result = await _orderService.GetByStatusAsync(status);
            return Ok(result);
        }

        // GET api/orders/user/5
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var result = await _orderService.GetByUserAsync(userId);
            return Ok(result);
        }

        // GET api/orders/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _orderService.GetByIdAsync(id);

            if (result is null)
                return NotFound(new { message = "Pedido no encontrado." });

            return Ok(result);
        }

        // POST api/orders
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderRequest request)
        {
            if (request.UserId <= 0)
                return BadRequest(new { message = "Usuario inválido." });

            if (request.Products is null || request.Products.Count == 0)
                return BadRequest(new { message = "El pedido debe tener al menos un producto." });

            if (string.IsNullOrEmpty(request.PaymentMethod))
                return BadRequest(new { message = "El método de pago es requerido." });

            if (string.IsNullOrEmpty(request.AddressSnapshot) || request.AddressSnapshot == "{}")
                return BadRequest(new { message = "La dirección de envío es requerida." });

            var result = await _orderService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT api/orders/5/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusRequest request)
        {
            if (string.IsNullOrEmpty(request.Status))
                return BadRequest(new { message = "El estado es requerido." });

            var success = await _orderService.UpdateStatusAsync(id, request);

            if (!success)
                return BadRequest(new { message = "Pedido no encontrado o estado inválido." });

            return Ok(new { message = "Estado actualizado correctamente." });
        }

    }
}
