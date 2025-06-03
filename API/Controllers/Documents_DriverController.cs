using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocument_DriverService _documentService;

        public DocumentsController(IDocument_DriverService documentService)
        {
            _documentService = documentService;
        }

        // GET /api/drivers/{driverId}/documents
        /// <summary>
        /// Obtiene todos los documentos asociados a un conductor específico.
        /// </summary>
        [HttpGet("drivers/{driverId}/documents", Name = "GetDriverDocuments")]
        [ProducesResponseType(typeof(IEnumerable<DocumentDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDocumentsByDriver(string driverId)
        {
            const string currentTenantId = "TENANT_PRUEBA_1";
            var documents = await _documentService.GetDocumentsByDriverIdAsync(driverId, currentTenantId);
            return Ok(documents);
        }

        // POST /api/drivers/{driverId}/documents
        /// <summary>
        /// Añade un nuevo documento a un conductor existente.
        /// </summary>
        [HttpPost("drivers/{driverId}/documents")]
        [ProducesResponseType(typeof(DocumentDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddDocumentToDriver(string driverId, [FromBody] DocumentCreateDTO documentCreateDto)
        {
            const string currentTenantId = "TENANT_PRUEBA_1";
            const string currentUserId = "USUARIO_SISTEMA_DOC";
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdDocument = await _documentService.AddDocumentAsync(driverId, documentCreateDto, currentTenantId, currentUserId);

            // Apuntar al endpoint que obtiene un documento específico (dentro de este controlador)
            return CreatedAtAction(nameof(GetSpecificDocumentForDriver),
                                  new { driverId = driverId, documentId = createdDocument.id_Document },
                                  createdDocument);
        }

        // GET /api/drivers/{driverId}/documents/{documentId}
        /// <summary>
        /// Obtiene un documento específico de un conductor específico.
        /// </summary>
        [HttpGet("drivers/{driverId}/documents/{documentId}", Name = "GetSpecificDocumentForDriver")]
        [ProducesResponseType(typeof(DocumentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSpecificDocumentForDriver(string driverId, string documentId)
        {
            const string currentTenantId = "TENANT_PRUEBA_1";
            var documentDto = await _documentService.GetDocumentByIdAsync(driverId, documentId, currentTenantId);
            if (documentDto == null) return NotFound(new { message = $"No se encontró documento con ID '{documentId}' para el conductor '{driverId}' en este inquilino." });
            return Ok(documentDto);
        }

        // PUT /api/drivers/{driverId}/documents/{documentId}
        /// <summary>
        /// Actualiza un documento específico de un conductor.
        /// </summary>
        [HttpPut("drivers/{driverId}/documents/{documentId}")]
        [ProducesResponseType(typeof(DocumentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateDriverDocument(string driverId, string documentId, [FromBody] DocumentUpdateDTO documentUpdateDto)
        {
            const string currentTenantId = "TENANT_PRUEBA_1";
            const string currentUserId = "USUARIO_SISTEMA_DOC_UPD";
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updatedDocument = await _documentService.UpdateDocumentAsync(driverId, documentId, documentUpdateDto, currentTenantId, currentUserId);
            return Ok(updatedDocument);
        }

        // DELETE /api/drivers/{driverId}/documents/{documentId}
        /// <summary>
        /// Elimina un documento específico de un conductor.
        /// </summary>
        [HttpDelete("drivers/{driverId}/documents/{documentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteDriverDocument(string driverId, string documentId)
        {
            const string currentTenantId = "TENANT_PRUEBA_1";
            var success = await _documentService.DeleteDocumentAsync(driverId, documentId, currentTenantId);
            if (success) return NoContent();
            else return NotFound(new { message = $"No se encontró documento con ID '{documentId}' para el conductor '{driverId}' en este inquilino, o no se pudo eliminar." });
        }
    }

}