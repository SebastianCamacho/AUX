using ENTITY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IDriverRepository
    {
        /// <summary>
        /// Obtiene un conductor por su ID (cédula), incluyendo sus documentos.
        /// </summary>
        /// <param name="driverId">El ID (cédula) del conductor.</param>
        /// <returns>El conductor encontrado o null.</returns>
        Task<Driver?> GetByIdAsync(string driverId);

        /// <summary>
        /// Obtiene todos los conductores, incluyendo sus documentos.
        /// </summary>
        /// <returns>Una colección de todos los conductores.</returns>
        Task<IEnumerable<Driver>> GetAllAsync();

        /// <summary>
        /// Añade un nuevo conductor a la base de datos.
        /// </summary>
        /// <param name="driver">La entidad conductor a añadir.</param>
        /// <returns>La entidad conductor añadida (con cualquier valor generado por la BD, si aplica).</returns>
        Task<Driver> AddAsync(Driver driver);

        /// <summary>
        /// Actualiza un conductor existente en la base de datos.
        /// </summary>
        /// <param name="driver">La entidad conductor con los datos actualizados.</param>
        /// <returns>La entidad conductor actualizada.</returns>
        Task<Driver> UpdateAsync(Driver driver);

        /// <summary>
        /// Elimina un conductor de la base de datos por su ID (cédula).
        /// </summary>
        /// <param name="driverId">El ID (cédula) del conductor a eliminar.</param>
        /// <returns>True si se eliminó, False si no se encontró.</returns>
        Task<bool> DeleteAsync(string driverId);
    }
}
