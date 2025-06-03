using AutoMapper;
using BLL.DTOs;
using BLL.Exceptions;
using BLL.Interfaces;
using DAL.Interfaces;
using ENTITY.Models;
using ENTITY.Models_Global;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{

    public class Document_DriverService : IDocument_DriverService
    {
        private readonly IDriverRepository _driverRepository; // Para validar el Driver padre
        private readonly IDocumentDriverRepository _documentDriverRepository; // Solo el repo interno
        private readonly IMapper _mapper;
        // private readonly ILogger<DocumentService> _logger;

        public Document_DriverService(
            IDriverRepository driverRepository,
            IDocumentDriverRepository documentDriverRepository,
            IMapper mapper /*, ILogger<DocumentService> logger */)
        {
            _driverRepository = driverRepository;
            _documentDriverRepository = documentDriverRepository;
            _mapper = mapper;
            // _logger = logger;
        }

        // --- Método Helper para validar el Driver (solo interno ahora) ---
        private async Task<Driver> ValidateDriverAsync(string driverId, string tenantId)
        {
            var driver = await _driverRepository.GetByIdAsync(driverId);
            if (driver == null)
            {
                // Si Driver tuviera un campo TenantId, la validación sería:
                // if (driver == null || driver.TenantId != tenantId)
                throw new BusinessException($"Conductor con cédula '{driverId}' no encontrado para el inquilino '{tenantId}'.", System.Net.HttpStatusCode.NotFound);
            }
            // Aquí también validaríamos if (driver.TenantId != tenantId) si Driver tuviera TenantId.
            return driver;
        }

        public async Task<DocumentDTO> AddDocumentAsync(string driverId, DocumentCreateDTO docDto, string tenantId, string createdBy)
        {
            // 1. Valido que el conductor al que voy a añadir el documento exista para este tenant.
            await ValidateDriverAsync(driverId, tenantId); // Lanza excepción si no.

            // 2. Verifico si este conductor ya tiene un documento con el mismo id_Document.
            //    GetByIdAsync en DocumentDriverRepository busca por la PK (id_Document).
            var existingDocument = await _documentDriverRepository.GetByIdAsync(docDto.id_Document);
            if (existingDocument != null && existingDocument.driver_Id == driverId)
            {
                // El documento con este ID ya existe Y pertenece a este mismo conductor.
                throw new BusinessException($"El conductor '{driverId}' ya tiene un documento con ID '{docDto.id_Document}'.", System.Net.HttpStatusCode.Conflict);
            }
            // Si existingDocument existe PERO existingDocument.driver_Id != driverId,
            // significa que otro conductor está usando ese id_Document.
            // Si id_Document debe ser único en toda la tabla Document_Driver, este es el lugar para esa validación.
            // Por ahora, la PK id_Document lo hará único en la tabla.

            // 3. Mapeo y preparo la entidad Document_Driver.
            var documentEntity = _mapper.Map<Document_Driver>(docDto);
            documentEntity.driver_Id = driverId; // Asigno el FK al conductor.
                                                 // No hay campos de auditoría en Document_Driver según la definición actual.

            // 4. Guardo el nuevo documento.
            Document_Driver savedDocument;
            try
            {
                savedDocument = await _documentDriverRepository.AddAsync(documentEntity);
            }
            catch (DbUpdateException ex) // Ej: si id_Document (PK) ya existiera en la tabla (raro si la validación anterior pasó)
            {
                // _logger.LogError(ex, "Error de BD al añadir documento {DocId} para driver {DriverId}", docDto.id_Document, driverId);
                throw new BusinessException($"Error al guardar el documento '{docDto.id_Document}'.", System.Net.HttpStatusCode.InternalServerError, ex);
            }

            // 5. Devuelvo el DTO del documento creado.
            return _mapper.Map<DocumentDTO>(savedDocument);
        }

        public async Task<DocumentDTO?> GetDocumentByIdAsync(string driverId, string documentId, string tenantId)
        {
            // 1. Valido el conductor.
            await ValidateDriverAsync(driverId, tenantId);

            // 2. Busco el documento por su PK (id_Document).
            var document = await _documentDriverRepository.GetByIdAsync(documentId);

            // 3. Verifico que el documento exista y que realmente pertenezca al driverId especificado.
            if (document == null || document.driver_Id != driverId)
            {
                return null; // No encontrado o no pertenece a este driver.
            }

            return _mapper.Map<DocumentDTO>(document);
        }

        public async Task<IEnumerable<DocumentDTO>> GetDocumentsByDriverIdAsync(string driverId, string tenantId)
        {
            // 1. Valido el conductor.
            await ValidateDriverAsync(driverId, tenantId);

            // 2. Obtengo los documentos del repositorio interno.
            var documents = await _documentDriverRepository.GetByDriverIdAsync(driverId);

            // 3. Mapeo y devuelvo. (Ya no hay lista global que añadir/comentar).
            return _mapper.Map<IEnumerable<DocumentDTO>>(documents);
        }

        public async Task<DocumentDTO> UpdateDocumentAsync(string driverId, string documentId, DocumentUpdateDTO docDto, string tenantId, string updatedBy)
        {
            // 1. Valido el conductor.
            await ValidateDriverAsync(driverId, tenantId);

            // 2. Busco el documento que voy a actualizar.
            var documentToUpdate = await _documentDriverRepository.GetByIdAsync(documentId);
            if (documentToUpdate == null || documentToUpdate.driver_Id != driverId)
            {
                throw new BusinessException($"Documento con ID '{documentId}' no encontrado para el conductor '{driverId}'.", System.Net.HttpStatusCode.NotFound);
            }

            // 3. Aplico los cambios del DTO a la entidad.
            // AutoMapper (con ForAllMembers Condition) solo actualiza campos no nulos del DTO.
            _mapper.Map(docDto, documentToUpdate);

            // 4. Aplico la lógica de negocio para is_Expirable y end_validity (como la teníamos).
            if (documentToUpdate.is_Expirable == false) // Si el estado final es no expirable
            {
                documentToUpdate.end_validity = null;
            }
            else // Si el estado final es expirable
            {
                if (!documentToUpdate.end_validity.HasValue) // Y no tiene fecha de fin
                {
                    throw new BusinessException("La fecha de finalización (end_validity) es requerida si el documento es expirable.", System.Net.HttpStatusCode.BadRequest);
                }
            }
            // No hay campos de auditoría en Document_Driver para 'updatedBy'.

            // 5. Guardo los cambios.
            await _documentDriverRepository.UpdateAsync(documentToUpdate);

            // 6. Devuelvo el DTO actualizado.
            return _mapper.Map<DocumentDTO>(documentToUpdate);
        }

        public async Task<bool> DeleteDocumentAsync(string driverId, string documentId, string tenantId)
        {
            // 1. Valido el conductor.
            await ValidateDriverAsync(driverId, tenantId);

            // 2. Verifico que el documento exista Y pertenezca a este driver antes de borrar.
            var documentToDelete = await _documentDriverRepository.GetByIdAsync(documentId);
            if (documentToDelete == null || documentToDelete.driver_Id != driverId)
            {
                return false; // No existe o no pertenece a este driver.
            }

            // 3. Borro el documento.
            return await _documentDriverRepository.DeleteAsync(documentId); // DeleteAsync del repo usa la PK (id_Document).
        }
    }



}
