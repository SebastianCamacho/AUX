using ENTITY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IDocumentDriverRepository
    {
        /// <summary>
        /// Obtiene un documento de conductor por su ID (PK).
        /// </summary>
        Task<Document_Driver?> GetByIdAsync(string idDocument);

        /// <summary>
        /// Obtiene todos los documentos asociados a un driverId específico.
        /// </summary>
        Task<IEnumerable<Document_Driver>> GetByDriverIdAsync(string driverId);

        /// <summary>
        /// Obtiene documentos para una lista de driverIds (usado para optimización N+1).
        /// </summary>
        Task<IEnumerable<Document_Driver>> GetByDriverIdsAsync(IEnumerable<string> driverIds);

        /// <summary>
        /// Añade un nuevo documento de conductor al contexto. NO llama a SaveChanges.
        /// </summary>
        Task<Document_Driver> AddAsync(Document_Driver entity);

        /// <summary>
        /// Añade una colección de nuevos documentos de conductor al contexto. NO llama a SaveChanges.
        /// </summary>
        Task AddRangeAsync(IEnumerable<Document_Driver> entities);

        /// <summary>
        /// Marca un documento de conductor como modificado en el contexto. NO llama a SaveChanges.
        /// </summary>
        Task<Document_Driver> UpdateAsync(Document_Driver entity);

        /// <summary>
        /// Marca un documento de conductor para eliminación basado en su ID (PK).
        /// NO llama a SaveChanges. Devuelve true si se encontró y marcó, false si no.
        /// </summary>
        Task<bool> DeleteAsync(string idDocument);
    }
}
