using LuxeCatalog.Business.DTOs.Users;
using LuxeCatalog.Business.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LuxeCatalog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET api/users
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllAsync();
            return Ok(result);
        }


        // GET api/users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _userService.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { message = "Usuario no encontrado" });

            return Ok(result);
        }

        // POST api/users
        [HttpPost("users")]

        public async Task<IActionResult> AddUser([FromBody]UserRequest request)
        {
            if (string.IsNullOrEmpty(request.FirstName))
                return BadRequest(new { message = "EL nombre es requerido" });

            if (string.IsNullOrEmpty(request.Email))
                return BadRequest(new { message = "El correo es requerido." });

            var result = await _userService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserRequest request)
        {
            if (string.IsNullOrEmpty(request.FirstName))
                return BadRequest(new { message = "El nombre es requerido" });
            if (string.IsNullOrEmpty(request.Email))
                return BadRequest(new { message = "El correo es requerido " });

            var result = await _userService.UpdateAsync(id, request);

            if (result is null)
                return NotFound(new { message = "Usuario no encontrado" });



            return Ok(result);
        }


        // PUT api/users/5/profile
        [HttpPut("{id}/profile")]
        public async Task<IActionResult> UpdateProfile(int id, [FromBody] UpdateProfileRequest request)
        {
            if (string.IsNullOrEmpty(request.FirstName))
                return BadRequest(new { message = "El nombre es requerido." });

            var result = await _userService.UpdateProfileAsync(id, request);

            if (result is null)
                return NotFound(new { message = "Usuario no encontrado." });

            return Ok(result);
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _userService.DeleteAsync(id);

            if (!success)
                return BadRequest(new { message = "No se puede eliminar. El usuario tiene pedidos registrados." });

            return Ok(new { message = "Usuario eliminado correctamente." });
        }

        // ── Direcciones ────────────────────────────────────────

        // GET api/users/5/addresses
        [HttpGet("{userId}/addresses")]
        public async Task<IActionResult> GetAddresses(int userId)
        {
            var result = await _userService.GetAddressesAsync(userId);
            return Ok(result);
        }

        // POST api/users/5/addresses
        [HttpPost("{userId}/addresses")]
        public async Task<IActionResult> AddAddress(int userId, [FromBody] AddressRequest request)
        {
            if (string.IsNullOrEmpty(request.Street))
                return BadRequest(new { message = "La calle es requerida." });

            var result = await _userService.AddAddressAsync(userId, request);
            return Ok(result);
        }

        // PUT api/users/5/addresses/3
        [HttpPut("{userId}/addresses/{addressId}")]
        public async Task<IActionResult> UpdateAddress(int userId, int addressId, [FromBody] AddressRequest request)
        {
            var result = await _userService.UpdateAddressAsync(userId, addressId, request);

            if (result is null)
                return NotFound(new { message = "Dirección no encontrada." });

            return Ok(result);
        }

        // DELETE api/users/5/addresses/3
        [HttpDelete("{userId}/addresses/{addressId}")]
        public async Task<IActionResult> DeleteAddress(int userId, int addressId)
        {
            var success = await _userService.DeleteAddressAsync(userId, addressId);

            if (!success)
                return NotFound(new { message = "Dirección no encontrada." });

            return Ok(new { message = "Dirección eliminada correctamente." });
        }

    }
}
