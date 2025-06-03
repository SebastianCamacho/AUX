using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IDocument_DriverService
    {
        /// <summary>
        /// Añade un nuevo documento a un conductor existente (solo en la BD interna).
        /// </summary>
        /// <param name="driverId">ID del conductor (cédula) al que se añade el documento.</param>
        /// <param name="docDto">Datos del documento a crear.</param>
        /// <param name="tenantId">ID del inquilino (para validación del conductor).</param>
        /// <param name="createdBy">Usuario que crea (actualmente no se usa en Document_Driver, pero por consistencia).</param>
        /// <returns>El DocumentDTO del documento creado.</returns>
        Task<DocumentDTO> AddDocumentAsync(string driverId, DocumentCreateDTO docDto, string tenantId, string createdBy);

        /// <summary>
        /// Obtiene un documento específico por su ID y el ID de su conductor.
        /// </summary>
        /// <param name="driverId">ID del conductor (cédula).</param>
        /// <param name="documentId">ID único del documento.</param>
        /// <param name="tenantId">ID del inquilino (para validación del conductor).</param>
        /// <returns>El DocumentDTO o null si no se encuentra.</returns>
        Task<DocumentDTO?> GetDocumentByIdAsync(string driverId, string documentId, string tenantId);

        /// <summary>
        /// Obtiene todos los documentos de un conductor específico (solo de la BD interna).
        /// </summary>
        /// <param name="driverId">ID del conductor (cédula).</param>
        /// <param name="tenantId">ID del inquilino (para validación del conductor).</param>
        /// <returns>Colección de DocumentDTOs.</returns>
        Task<IEnumerable<DocumentDTO>> GetDocumentsByDriverIdAsync(string driverId, string tenantId);

        /// <summary>
        /// Actualiza un documento existente asociado a un conductor (solo en la BD interna).
        /// </summary>
        /// <param name="driverId">ID del conductor (cédula).</param>
        /// <param name="documentId">ID del documento a actualizar.</param>
        /// <param name="docDto">Datos a actualizar.</param>
        /// <param name="tenantId">ID del inquilino (para validación del conductor).</param>
        /// <param name="updatedBy">Usuario que actualiza.</param>
        /// <returns>El DocumentDTO del documento actualizado.</returns>
        Task<DocumentDTO> UpdateDocumentAsync(string driverId, string documentId, DocumentUpdateDTO docDto, string tenantId, string updatedBy);

        /// <summary>
        /// Elimina un documento específico asociado a un conductor (solo de la BD interna).
        /// </summary>
        /// <param name="driverId">ID del conductor (cédula).</param>
        /// <param name="documentId">ID del documento a eliminar.</param>
        /// <param name="tenantId">ID del inquilino (para validación del conductor).</param>
        /// <returns>True si se eliminó, False si no se encontró.</returns>
        Task<bool> DeleteDocumentAsync(string driverId, string documentId, string tenantId);
    }
}
