using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ENTITY;
using Backend_Asistencias.DTOs;
using Backend_Asistencias.Services;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Backend_Asistencias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsistenciasController : ControllerBase
    {
        private readonly Plataforma_Context _context;
        private readonly IAuthServices _authServices;
        private readonly INotificacionService _notificacionService;

        public AsistenciasController(Plataforma_Context context, IAuthServices authServices, INotificacionService notificacionService)
        {
            _context = context;
            _authServices = authServices;
            _notificacionService = notificacionService;
        }

        [HttpGet]
        [Route("Listar")]
        public async Task<ActionResult<IEnumerable<AsistenciaDTO>>> GetAsistencias()
        {
            // 1. Extraer el token del encabezado 'Authorization'
            if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                return Unauthorized("Token no proporcionado");
            }
            try
            {
                var token = tokenHeader.ToString().Replace("Bearer ", "");
                var authAdmin = await _authServices.validarAdmin(token);
                var authStudent = await _authServices.validarEstudent(token);
                var authProfesor = await _authServices.validarProfesor(token);
                if (!authAdmin.IsSuccessStatusCode && !authProfesor.IsSuccessStatusCode && !authStudent.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "ACCESO DENEGADO" });
                }
                var asistencias = await _context.Asistencias
                    .Include(a => a.Estudiante)
                    .Include(a => a.Clase)
                    .Select(a => new AsistenciaDTO
                    {
                        Fecha = a.Fecha,
                        Estado = a.Estado,
                        estudiante = new EstudianteDTO
                        {
                            Identificacion = a.Estudiante.Identificacion,
                            Nombres = a.Estudiante.Nombres,
                            Apellidos = a.Estudiante.Apellidos,
                            Foto = a.Estudiante.Foto,
                            Carrera = a.Estudiante.Carrera,
                        },
                        clase = new ClaseDTO
                        {
                            IdClase = a.Clase.idClase,
                            Fecha = a.Clase.fecha,
                            Grupo =  new GrupoDTO
                            {
                                IdGrupo = a.Clase.grupo.idGrupo,
                                Descripcion = a.Clase.grupo.Descripcion,
                                asignatura = a.Clase.grupo.asignatura,
                            }                            
                        }
                    }).ToListAsync();

                if (asistencias == null || !asistencias.Any())
                {
                    return NotFound("No se encontraron asistencias.");
                }

                return Ok(asistencias);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener las asistencias: {ex.Message}");
            }
        }



        [HttpGet("ListarId/{id}")]
        public async Task<ActionResult<AsistenciaDTO>> GetAsistencia(int id)
        {
            // 1. Extraer el token del encabezado 'Authorization'
            if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                return Unauthorized("Token no proporcionado");
            }
            try
            {
                var token = tokenHeader.ToString().Replace("Bearer ", "");
                var authAdmin = await _authServices.validarAdmin(token);
                var authStudent = await _authServices.validarEstudent(token);
                var authProfesor = await _authServices.validarProfesor(token);
                if (!authAdmin.IsSuccessStatusCode && !authProfesor.IsSuccessStatusCode && !authStudent.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "ACCESO DENEGADO" });
                }
                var asistencia = await _context.Asistencias
                    .Include(a => a.Estudiante)
                    .Include(a => a.Clase)
                    .FirstOrDefaultAsync(a => a.Clase.idClase == id);

                if (asistencia == null)
                {
                    return NotFound($"No se encontró asistencia con el ID {id}.");
                }

                var asistenciaDto = new AsistenciaDTO
                {
                    Fecha = asistencia.Fecha,
                    Estado = asistencia.Estado,
                    estudiante = new EstudianteDTO
                    {
                        Identificacion = asistencia.Estudiante.Identificacion,
                        Nombres = asistencia.Estudiante.Nombres,
                        Apellidos = asistencia.Estudiante.Apellidos
                    },
                    clase = new ClaseDTO
                    {
                        IdClase = asistencia.Clase.idClase,
                        Fecha = asistencia.Clase.fecha,
                        Grupo = new GrupoDTO
                        {
                            IdGrupo = asistencia.Clase.grupo.idGrupo,
                            Descripcion = asistencia.Clase.grupo.Descripcion,
                            asignatura = asistencia.Clase.grupo.asignatura,
                        }
                    }
                };

                return Ok(asistenciaDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener la asistencia: {ex.Message}");
            }
        }



        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> PostAsistencia(AsistenciaCreateDTO asistenciaDto)
        {
            // 1. Extraer el token del encabezado 'Authorization'
            if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                return Unauthorized("Token no proporcionado");
            }
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var token = tokenHeader.ToString().Replace("Bearer ", "");
                var authAdmin = await _authServices.validarAdmin(token);
                var authProfesor = await _authServices.validarProfesor(token);
                if (!authAdmin.IsSuccessStatusCode && !authProfesor.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "ACCESO DENEGADO" });
                }
                var nuevaAsistencia = new Asistencia
                {
                    Fecha = asistenciaDto.Fecha,
                    Estado = asistenciaDto.Estado,
                    EstudianteIdentificacion = asistenciaDto.EstudianteIdentificacion,
                    idClase = asistenciaDto.idClase
                };

                _context.Asistencias.Add(nuevaAsistencia);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var user = await _context.Users
                    .Include(u => u.Usuario)
                    .FirstOrDefaultAsync(u => u.UsuarioIdentificacion == asistenciaDto.EstudianteIdentificacion);
                var asignatura = await _context.Clases
                    .Include(c=>c.grupo).ThenInclude(g => g.asignatura)
                    .Where(c => c.idClase == asistenciaDto.idClase)                // Filtra por el id de la clase
                    .Select(c => c.grupo.asignatura)                 // Selecciona la asignatura a través de la relación con el grupo
                    .FirstOrDefaultAsync();                          // Devuelve la primera asignatura encontrada o null si no hay

                

                string body;
                if (asistenciaDto.Estado) {
                    body = "Asistencia registrada con exito en la asignatura: " +
                        $"{asignatura.NombreAsignatura} el dia {asistenciaDto.Fecha.ToLocalTime()}" ;
                }
                else
                {
                    body = "Falla registrada con exito en la asignatura: " +
                        $"{asignatura.NombreAsignatura} el dia {asistenciaDto.Fecha.ToLocalTime()}";
                }
                

                Correo _correo = new Correo
                {
                    ToEmail = user.email,
                    Subject = asistenciaDto.Estado ?"ASISTENCIA REGISTRADA":"FALLA REGISTRADA" ,
                    Body = body
                };

                string message = await _notificacionService.sendCorreo(_correo);


                return StatusCode(StatusCodes.Status201Created, new { message = "Asistencia creada correctamente.", response = nuevaAsistencia });
            }
            
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();
                // Manejo específico para errores de base de datos
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al guardar en la base de datos: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Manejo de cualquier otra excepción general
                return StatusCode(StatusCodes.Status400BadRequest, $"Error al procesar la solicitud: {ex.Message}");
            }
        }


        [HttpPut("Editar/{estudianteId}/{idClase}")]
        public async Task<ActionResult> EditarAsistencia(string estudianteId, int idClase, AsistenciaCreateDTO asistenciaEditada)
        {
            // Validar que los IDs coincidan
            

            // 1. Extraer el token del encabezado 'Authorization'
            if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                return Unauthorized("Token no proporcionado");
            }
            if (estudianteId != asistenciaEditada.EstudianteIdentificacion || idClase != asistenciaEditada.idClase)
            {
                return BadRequest("Los IDs proporcionados no coinciden con los de la URL.");
            }
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                var token = tokenHeader.ToString().Replace("Bearer ", "");
                var authAdmin = await _authServices.validarAdmin(token);
                var authProfesor = await _authServices.validarProfesor(token);
                if (!authAdmin.IsSuccessStatusCode && !authProfesor.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "ACCESO DENEGADO" });
                }
                // Intentar encontrar la asistencia existente
                var asistenciaExistente = await _context.Asistencias
                    .Include(a => a.Estudiante)
                    .Include(a => a.Clase)
                    .FirstOrDefaultAsync(a => a.EstudianteIdentificacion == estudianteId && a.idClase == idClase);

                // Si no existe, crear una nueva asistencia
                if (asistenciaExistente == null)
                {
                    var nuevaAsistencia = new Asistencia
                    {
                        Fecha = asistenciaEditada.Fecha,
                        Estado = asistenciaEditada.Estado,
                        EstudianteIdentificacion = estudianteId,
                        idClase = idClase
                    };

                    _context.Asistencias.Add(nuevaAsistencia);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    // Devuelve un mensaje de éxito
                    return StatusCode(StatusCodes.Status201Created, new { message = "Asistencia creada con éxito", response = nuevaAsistencia });
                }
                else
                {
                    // Actualiza los datos de la asistencia existente
                    asistenciaExistente.Fecha = asistenciaEditada.Fecha;
                    asistenciaExistente.Estado = asistenciaEditada.Estado;

                    await _context.SaveChangesAsync();

                    // Devuelve un mensaje de éxito
                    return StatusCode(StatusCodes.Status200OK, new { message = "Asistencia editada con éxito", response = asistenciaExistente });
                }
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error de concurrencia al actualizar la asistencia: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al procesar la solicitud: {ex.Message}");
            }
        }


        [HttpDelete("Eliminar/{estudianteId}/{idClase}")]
        public async Task<ActionResult> EliminarAsistencia(string estudianteId, int idClase)
        {
            if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                return Unauthorized("Token no proporcionado");
            }
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var token = tokenHeader.ToString().Replace("Bearer ", "");
                var authAdmin = await _authServices.validarAdmin(token);
                if (!authAdmin.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "ACCESO DENEGADO" });
                }
                var asistencia = await _context.Asistencias
                    .Include(a => a.Estudiante)
                    .Include(a => a.Clase)
                    .FirstOrDefaultAsync(a => a.EstudianteIdentificacion == estudianteId && a.idClase == idClase);

                if (asistencia == null)
                {
                    return NotFound($"No se encontró una asistencia para el estudiante con la identificación {estudianteId} y clase {idClase}.");
                }

                _context.Asistencias.Remove(asistencia);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                // Devuelve un mensaje de éxito
                return StatusCode(StatusCodes.Status200OK, new { message = "Asistencia eliminada con éxito" });
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al eliminar la asistencia: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al procesar la solicitud: {ex.Message}");
            }
        }

        private bool AsistenciaExists(string id)
        {
            return _context.Asistencias.Any(e => e.EstudianteIdentificacion == id);
        }
    }
}
