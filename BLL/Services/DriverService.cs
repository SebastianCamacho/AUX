using AutoMapper;
using BLL.DTOs;
using BLL.Exceptions;
using BLL.Interfaces;
using DAL;
using DAL.Interfaces;
using DAL.Interfaces_Histories;
using ENTITY.Models;
using ENTITY.Models_Global;
using ENTITY.Models_Histories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BLL.Services
{
    public class DriverService : IDriverService
    {
        private readonly FUEC_DbContext _context; // Para transacciones y SaveChangesAsync
        private readonly IDriverRepository _driverRepository;
        private readonly IDocumentDriverRepository _documentDriverRepository;
        private readonly IDriverHistoryRepository _driverHistoryRepository;
        private readonly IDocumentDriverHistoryRepository _documentDriverHistoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DriverService> _logger;

        public DriverService(
            FUEC_DbContext context, // Inyectamos el DbContext
            IDriverRepository driverRepository,
            IDocumentDriverRepository documentDriverRepository,
            IDriverHistoryRepository driverHistoryRepository,
            IDocumentDriverHistoryRepository documentDriverHistoryRepository,
            IMapper mapper, 
            ILogger<DriverService> logger)
        {
            _context = context; // Guardamos la instancia del DbContext
            _driverRepository = driverRepository;
            _documentDriverRepository = documentDriverRepository;
            _driverHistoryRepository = driverHistoryRepository;
            _documentDriverHistoryRepository = documentDriverHistoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<DriverDTO> CreateDriverAsync(DriverCreateDTO dto, string tenantId, string createdBy)
        {
            var existingDriver = await _driverRepository.GetByIdAsync(dto.driver_Id);
            if (existingDriver != null)
            {
                throw new BusinessException($"Ya existe un conductor con la cédula '{dto.driver_Id}'.", System.Net.HttpStatusCode.Conflict);
            }

            var driverEntity = _mapper.Map<Driver>(dto);
            driverEntity.state = true;
            driverEntity.create_At = DateTime.UtcNow;
            driverEntity.update_At = DateTime.UtcNow;
            driverEntity.create_By = createdBy;
            driverEntity.update_By = createdBy;
            // if (driverEntity tiene TenantId) driverEntity.TenantId = tenantId;

            var driverHistoryEntry = _mapper.Map<Driver_History>(driverEntity);
            driverHistoryEntry.DriverAuditId = driverEntity.driver_Id;
            driverHistoryEntry.ActionTimestamp = driverEntity.create_At;
            driverHistoryEntry.ActionType = "CREADO";
            driverHistoryEntry.ChangedByUserId = createdBy;
            // driverHistoryEntry.SnapshottedDocuments se inicializa como lista vacía.

            var documentEntitiesToProcess = new List<Document_Driver>();

            if (dto.Documents != null && dto.Documents.Any())
            {
                foreach (var docDto in dto.Documents)
                {
                    var documentEntity = _mapper.Map<Document_Driver>(docDto);
                    documentEntity.driver_Id = driverEntity.driver_Id;
                    documentEntitiesToProcess.Add(documentEntity); // Para guardarlos en su tabla

                    var docHistoryEntry = _mapper.Map<Document_Driver_History>(documentEntity);
                    docHistoryEntry.ActionTimestamp = driverHistoryEntry.ActionTimestamp;
                    docHistoryEntry.ActionType = "CREADO";
                    docHistoryEntry.ChangedByUserId = createdBy;
                    // NO asignamos DriverHistoryId_FK aquí. EF Core lo hará.

                    // Añadimos el historial del documento a la colección del historial del driver
                    driverHistoryEntry.SnapshottedDocuments.Add(docHistoryEntry);
                }
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // 1. Añadir entidades principales y sus dependientes directas al contexto
                    // Los repositorios solo hacen _context.Add() o _context.AddRange()
                    await _driverRepository.AddAsync(driverEntity);
                    await _driverHistoryRepository.AddAsync(driverHistoryEntry); // Esto incluye los Document_Driver_History en SnapshottedDocuments

                    if (documentEntitiesToProcess.Any())
                    {
                        await _documentDriverRepository.AddRangeAsync(documentEntitiesToProcess);
                    }

                    // 2. UN ÚNICO SaveChanges para TODO
                    await _context.SaveChangesAsync();

                    // 3. Confirmar la transacción
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    // _logger.LogError(ex, $"Transacción fallida al crear conductor {dto.driver_Id}. Rollback ejecutado.");
                    if (ex is DbUpdateException dbEx && dbEx.InnerException is MySqlException mysqlEx && mysqlEx.Number == 1062)
                    {
                        throw new BusinessException($"Conflicto de datos al guardar. Verifique cédula o IDs de documento.", System.Net.HttpStatusCode.Conflict, ex);
                    }
                    throw new BusinessException("Error al crear el conductor y su historial. La operación fue revertida.", System.Net.HttpStatusCode.InternalServerError, ex);
                }
            }

            var resultDto = await GetDriverByIdAsync(driverEntity.driver_Id, tenantId);
            if (resultDto == null)
            {
                throw new BusinessException("Conductor creado exitosamente, pero no se pudo recuperar para confirmación.", System.Net.HttpStatusCode.InternalServerError);
            }
            return resultDto;
        }
       
        
        public async Task<DriverDTO?> GetDriverByIdAsync(string driverId, string tenantId)
        {
            // El tenantId aquí es conceptual, para validaciones futuras si Driver tuviera TenantId.
            // Por ahora, GetByIdAsync busca por la PK (cédula).
            var driver = await _driverRepository.GetByIdAsync(driverId);
            if (driver == null)
            {
                return null;
            }
            // Aquí iría la validación si Driver.TenantId existiera:
            // if (driver.TenantId != tenantId) { /* Log y/o lanzar Forbidden */ return null; }

            return _mapper.Map<DriverDTO>(driver);
        }

        public async Task<IEnumerable<DriverDTO>> GetAllDriversAsync(string tenantId)
        {
            // El tenantId aquí es para una posible lógica de filtrado futura si Driver tuviera TenantId.
            // Actualmente, GetAllAsync() del repo trae todos los drivers.
            var drivers = await _driverRepository.GetAllAsync();
            // Si Driver tuviera TenantId:
            // var drivers = (await _driverRepository.GetAllAsync()).Where(d => d.TenantId == tenantId);

            return _mapper.Map<IEnumerable<DriverDTO>>(drivers);
        }

        public async Task<DriverDTO> UpdateDriverAsync(string driverId, DriverUpdateDTO dto, string tenantId, string updatedBy)
        {
            // 1. Busco el conductor que se va a actualizar.
            var driverBeforeUpdate = await _driverRepository.GetByIdAsync(driverId);
            if (driverBeforeUpdate == null)
            {
                throw new BusinessException($"Conductor con cédula '{driverId}' no encontrado.", System.Net.HttpStatusCode.NotFound);
            }

            // (Aquí validación de tenant si fuera necesario)

            // Cargamos explícitamente los documentos actuales del conductor ---
            // Esto es para asegurar que tenemos la "foto" correcta de los documentos para el historial,
            // independientemente de si GetByIdAsync del _driverRepository los incluye o no.
            var currentLiveDocuments = await _documentDriverRepository.GetByDriverIdAsync(driverId);
            // --- FIN NUEVO ---

            // 2. Preparo la entrada de historial para el DRIVER (su estado ANTES de los cambios).
            var driverHistoryEntry = _mapper.Map<Driver_History>(driverBeforeUpdate);
            driverHistoryEntry.DriverAuditId = driverBeforeUpdate.driver_Id;
            driverHistoryEntry.ActionTimestamp = DateTime.UtcNow;
            driverHistoryEntry.ActionType = "ACTUALIZADO";
            driverHistoryEntry.ChangedByUserId = updatedBy;

            // 3. Preparo el historial para cada DOCUMENTO actual del conductor.
            if (currentLiveDocuments != null && currentLiveDocuments.Any()) // Usamos la lista que cargamos explícitamente
            {
                foreach (var liveDocument in currentLiveDocuments)
                {
                    var docHistoryEntry = _mapper.Map<Document_Driver_History>(liveDocument);
                    docHistoryEntry.ActionTimestamp = driverHistoryEntry.ActionTimestamp;
                    docHistoryEntry.ActionType = "SNAPSHOT_POR_UPDATE_DRIVER";
                    docHistoryEntry.ChangedByUserId = updatedBy;
                    // DocumentAuditId y driver_Id_Original se mapean desde liveDocument.
                    driverHistoryEntry.SnapshottedDocuments.Add(docHistoryEntry);
                }
            }

            // 4. Aplico los cambios del DTO (DriverUpdateDTO) al conductor (driverBeforeUpdate).
            _mapper.Map(dto, driverBeforeUpdate);

            // 5. Actualizo campos de auditoría del conductor "vivo".
            driverBeforeUpdate.update_At = driverHistoryEntry.ActionTimestamp;
            driverBeforeUpdate.update_By = updatedBy;

            // Inicio la transacción explícita
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // 6. Añado el Driver_History (que incluye sus Document_Driver_History hijos) al contexto.
                    await _driverHistoryRepository.AddAsync(driverHistoryEntry);

                    // 7. Marco el Driver "vivo" (que ahora tiene los cambios del DTO) como modificado.
                    await _driverRepository.UpdateAsync(driverBeforeUpdate);

                    // 8. UN ÚNICO SaveChanges para persistir el historial Y la actualización del Driver.
                    await _context.SaveChangesAsync();

                    // 9. Confirmo la transacción.
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new BusinessException("Error al actualizar el conductor o su historial. La operación fue revertida.", System.Net.HttpStatusCode.InternalServerError, ex);
                }
            }

            // 10. Devuelvo el DTO del conductor actualizado.
            var resultDto = await GetDriverByIdAsync(driverId, tenantId);
            if (resultDto == null)
            {
                throw new BusinessException("Conductor actualizado, pero no se pudo recuperar para confirmación.", System.Net.HttpStatusCode.InternalServerError);
            }
            return resultDto;
        }


        public async Task<bool> DeleteDriverAsync(string driverId, string tenantId)
        {
            // 1. Busco el conductor que voy a eliminar.
            // Necesito sus datos para el historial ANTES de que desaparezca.
            var driverToDelete = await _driverRepository.GetByIdAsync(driverId);
            if (driverToDelete == null)
            {
                // No se encontró, no hay nada que borrar.
                return false;
            }

            // Aquí validaría pertenencia al tenant si Driver.TenantId existiera.
            // if (driverToDelete.TenantId != tenantId)
            // {
            //     // _logger.LogWarning("Intento de borrar Driver '{DriverId}' que no pertenece al Tenant '{TenantId}'.", driverId, tenantId);
            //     return false; // O lanzar BusinessException(Forbidden)
            // }

            // --- INICIO: Guardar en Historial (ANTES de eliminar el principal) ---
            // Necesitamos un 'userId' para el campo ChangedByUserId. Como el método Delete no lo recibe,
            // usamos un placeholder o el tenantId. Idealmente, se pasaría un 'deletedByUserId'.
            // TODO: Considerar pasar 'deletedByUserId' al método del servicio.
            const string deletedByPlaceholder = "SISTEMA_O_TENANT"; // O podrías usar 'tenantId'

            var historyEntry = _mapper.Map<Driver_History>(driverToDelete); // Mapea el estado ANTES del delete
            historyEntry.DriverAuditId = driverToDelete.driver_Id;
            historyEntry.ActionTimestamp = DateTime.UtcNow; // Momento exacto de esta acción de "eliminación"
            historyEntry.ActionType = "ELIMINADO"; // Marcamos la acción
            historyEntry.ChangedByUserId = deletedByPlaceholder; // Quién está borrando
                                                                 // Los campos create_At_OriginalDriver, etc., ya se mapearon desde driverToDelete.

            try
            {
                await _driverHistoryRepository.AddAsync(historyEntry);
                // _logger.LogInformation("Registro de historial ELIMINADO para Driver {DriverId}", driverToDelete.driver_Id);
            }
            catch (Exception historyEx)
            {
                // Si falla el guardado del historial, ¿detenemos la eliminación principal?
                // Esto es una decisión de diseño. Si el historial es absolutamente crítico,
                // podrías lanzar una excepción aquí y no proceder con el borrado.
                // Por ahora, solo logueamos y continuamos con el borrado del driver principal.
                _logger.LogError(historyEx, "FALLO al guardar en Driver_History para Driver {DriverId} ANTES de eliminarlo.", driverToDelete.driver_Id);
                // Considera el impacto de no tener el registro de historial si el borrado principal sí funciona.
            }
            // --- FIN: Guardar en Historial ---

            // 2. Borro los documentos asociados al conductor (esto ya lo teníamos).
            // Es importante hacerlo ANTES de borrar el conductor para evitar problemas de FK.
            if (driverToDelete.Document_Drivers != null && driverToDelete.Document_Drivers.Any())
            {
                // _logger.LogInformation("Borrando {DocCount} documentos para el driver {DriverId} antes de borrar el driver.", driverToDelete.Document_Drivers.Count, driverId);
                foreach (var doc in driverToDelete.Document_Drivers.ToList()) // ToList() para evitar problemas al modificar colección mientras se itera
                {
                    try
                    {
                        await _documentDriverRepository.DeleteAsync(doc.id_Document);
                    }
                    catch (Exception docEx)
                    {
                        // Si falla el borrado de un documento, ¿qué hacemos?
                        // Por ahora, logueamos y continuamos. El borrado del driver es el objetivo principal.
                        _logger.LogError(docEx, $"Fallo al borrar Document_Driver {doc.id_Document} para Driver {driverToDelete.driver_Id}.", doc.id_Document, driverId);
                        // Considera si esto debería impedir el borrado del Driver.
                    }
                }
            }

            // 3. Borro el conductor de la tabla principal.
            // El método DeleteAsync del repositorio devuelve true si lo encontró y borró.
            var mainDeleteSuccess = await _driverRepository.DeleteAsync(driverId);

            if (!mainDeleteSuccess && historyEntry.HistoryId > 0)
            {
                // Esto sería una situación extraña: el historial se guardó, pero el driver principal
                // no se pudo borrar (quizás ya había sido borrado por otro proceso entre el GetByIdAsync y el DeleteAsync).
                // _logger.LogWarning("Se guardó historial de borrado para Driver {DriverId}, pero el borrado principal no lo encontró.", driverId);
            }

            return mainDeleteSuccess; // Devuelve el resultado del borrado del registro principal.
        }

    }


}

