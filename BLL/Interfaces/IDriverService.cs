using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IDriverService
    {
        /// <summary>
        /// Crea un nuevo conductor y sus documentos iniciales.
        /// </summary>
        /// <param name="dto">Datos para crear el conductor.</param>
        /// <param name="tenantId">ID del inquilino (contexto actual).</param>
        /// <param name="createdBy">ID del usuario que crea el registro.</param>
        /// <returns>El DTO del conductor creado.</returns>
        Task<DriverDTO> CreateDriverAsync(DriverCreateDTO dto, string tenantId, string createdBy);

        /// <summary>
        /// Obtiene un conductor por su ID (cédula) para un tenant específico.
        /// </summary>
        /// <param name="driverId">La cédula del conductor.</param>
        /// <param name="tenantId">ID del inquilino (contexto actual).</param>
        /// <returns>El DTO del conductor o null si no se encuentra.</returns>
        Task<DriverDTO?> GetDriverByIdAsync(string driverId, string tenantId);

        /// <summary>
        /// Obtiene todos los conductores para un tenant específico.
        /// </summary>
        /// <param name="tenantId">ID del inquilino (contexto actual).</param>
        /// <returns>Una colección de DTOs de conductores.</returns>
        Task<IEnumerable<DriverDTO>> GetAllDriversAsync(string tenantId); // Cambié el nombre para claridad

        /// <summary>
        /// Actualiza un conductor existente.
        /// </summary>
        /// <param name="driverId">La cédula del conductor a actualizar.</param>
        /// <param name="dto">Datos para actualizar.</param>
        /// <param name="tenantId">ID del inquilino (contexto actual).</param>
        /// <param name="updatedBy">ID del usuario que actualiza.</param>
        /// <returns>El DTO del conductor actualizado.</returns>
        Task<DriverDTO> UpdateDriverAsync(string driverId, DriverUpdateDTO dto, string tenantId, string updatedBy);

        /// <summary>
        /// Elimina un conductor y sus documentos asociados.
        /// </summary>
        /// <param name="driverId">La cédula del conductor a eliminar.</param>
        /// <param name="tenantId">ID del inquilino (contexto actual).</param>
        /// <returns>True si se eliminó, False si no se encontró.</returns>
        Task<bool> DeleteDriverAsync(string driverId, string tenantId);
    }
}
