using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly IDriverService _driverService; // Solo necesitamos este servicio aquí

        public DriversController(IDriverService driverService)
        {
            _driverService = driverService;
        }

        // POST /api/drivers (Crear Driver)
        [HttpPost]
        public async Task<IActionResult> CreateDriver([FromBody] DriverCreateDTO driverCreateDto)
        {
            const string currentTenantId = "TENANT_PRUEBA_1"; // TODO: Obtener de autenticación
            const string currentUserId = "USUARIO_SISTEMA";  // TODO: Obtener de autenticación

            if (!ModelState.IsValid) return BadRequest(ModelState);

            DriverDTO createdDriver = await _driverService.CreateDriverAsync(driverCreateDto, currentTenantId, currentUserId);
            return CreatedAtAction(nameof(GetDriverById), new { driverId = createdDriver.driver_Id }, createdDriver);
        }

        // GET /api/drivers/{driverId} (Obtener Driver por ID)
        [HttpGet("{driverId}", Name = "GetDriverById")]
        public async Task<IActionResult> GetDriverById(string driverId)
        {
            const string currentTenantId = "TENANT_PRUEBA_1"; // TODO: Obtener de autenticación
            DriverDTO? driverDto = await _driverService.GetDriverByIdAsync(driverId, currentTenantId);
            if (driverDto == null) return NotFound(new { message = $"No se encontró un conductor con ID '{driverId}' para este inquilino." });
            return Ok(driverDto);
        }

        // GET /api/drivers (Obtener todos los Drivers del Tenant)
        [HttpGet]
        public async Task<IActionResult> GetDrivers()
        {
            const string currentTenantId = "TENANT_PRUEBA_1"; // TODO: Obtener de autenticación
            IEnumerable<DriverDTO> drivers = await _driverService.GetAllDriversAsync(currentTenantId); // Usa el método renombrado del servicio
            return Ok(drivers);
        }

        // PUT /api/drivers/{driverId} (Actualizar Driver)
        [HttpPut("{driverId}")]
        public async Task<IActionResult> UpdateDriver(string driverId, [FromBody] DriverUpdateDTO driverUpdateDto)
        {
            const string currentTenantId = "TENANT_PRUEBA_1"; // TODO: Obtener de autenticación
            const string currentUserId = "USUARIO_SISTEMA_UPDATE"; // TODO: Obtener de autenticación
            if (!ModelState.IsValid) return BadRequest(ModelState);
            DriverDTO updatedDriver = await _driverService.UpdateDriverAsync(driverId, driverUpdateDto, currentTenantId, currentUserId);
            return Ok(updatedDriver);
        }

        // DELETE /api/drivers/{driverId} (Eliminar Driver)
        [HttpDelete("{driverId}")]
        public async Task<IActionResult> DeleteDriver(string driverId)
        {
            const string currentTenantId = "TENANT_PRUEBA_1"; // TODO: Obtener de autenticación
            bool success = await _driverService.DeleteDriverAsync(driverId, currentTenantId);
            if (success) return NoContent();
            else return NotFound(new { message = $"No se encontró un conductor con ID '{driverId}' para este inquilino." });
        }

    }
}
