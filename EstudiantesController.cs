using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ENTITY;
using Backend_Asistencias.DTOs;
using Microsoft.AspNetCore.Authorization;
using Backend_Asistencias.Services;

namespace Backend_Asistencias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiantesController : ControllerBase
    {
        private readonly Plataforma_Context _context;
        private readonly IAuthServices _auhtServices;

        public EstudiantesController(Plataforma_Context context, IAuthServices authServices)
        {
            _context = context;
            _auhtServices = authServices;

        }

        // GET: api/Estudiantes/Listar
        [HttpGet]
        [Route("Listar")]
        public async Task<ActionResult<IEnumerable<EstudianteDTO>>> GetEstudiantes()
        {
            // 1. Extraer el token del encabezado 'Authorization'
            if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                return Unauthorized("Token no proporcionado");
            }
            try
            {
                var token = tokenHeader.ToString().Replace("Bearer ", "");
                var resultadoApiExterna = await _auhtServices.validarAdmin(token);
                if (!resultadoApiExterna.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "ACCESO DENEGADO" });
                }
                var estudiantes = await _context.Estudiantes
                    .Include(e => e.Matriculas).ThenInclude(m => m.Grupo) // Incluye las matrículas relacionadas
                    .Include(e => e.Asistencias)
                    .Select(e => new EstudianteDTO
                    {
                        Identificacion = e.Identificacion,
                        Nombres = e.Nombres,
                        Apellidos = e.Apellidos,
                        Foto = e.Foto,
                        Carrera = e.Carrera,
                        Matriculas = e.Matriculas.Select(m => new MatriculaDTO
                        {
                            FechaMatricula = m.fechaMatricula,
                            GrupoidGrupo = m.GrupoidGrupo,
                            grupo = new GrupoDTO
                            {
                                IdGrupo = m.Grupo.idGrupo,
                                Descripcion = m.Grupo.Descripcion,
                                asignatura = m.Grupo.asignatura,
                                Horarios = m.Grupo.Horarios.Select(c => new HorarioDTO
                                {
                                    idHorario = c.idHorario,
                                    diaSemana = c.diaSemana,
                                    horaInicio = c.horaInicio,
                                    minutosInicio = c.minutosInicio,
                                    horaFin = c.horaFin,
                                    minutosFin = c.minutosFin,


                                }).ToList()

                            },
                        }).ToList(),
                        Asistencias = e.Asistencias.Select(m => new AsistenciaDTO
                        {
                            Fecha = m.Fecha,
                            Estado = m.Estado,
                            clase = new ClaseDTO
                            {
                                IdClase = m.Clase.idClase,
                                Fecha = m.Clase.fecha,
                                Grupo = new GrupoDTO
                                {
                                    IdGrupo = m.Clase.grupo.idGrupo,
                                    Descripcion = m.Clase.grupo.Descripcion,
                                    asignatura = m.Clase.grupo.asignatura
                                },
                            },
                        }).ToList()


                    })
                    .ToListAsync();

                if (estudiantes == null || !estudiantes.Any())
                {
                    return NotFound("No se encontraron estudiantes.");
                }

                // Devuelve un HTTP 200 (OK) con la lista de estudiantes
                return Ok(estudiantes);
            }
            catch (Exception ex)
            {
                // Manejo de cualquier excepción general
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener los estudiantes: {ex.Message}");
            }
        }


        // GET: api/Estudiantes/ListarId/5
        [HttpGet("ListarId/{id}")]
        public async Task<ActionResult<EstudianteDTO>> GetEstudiante(string id)
        {
            // 1. Extraer el token del encabezado 'Authorization'
            if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                return Unauthorized("Token no proporcionado");
            }
            try
            {
                var token = tokenHeader.ToString().Replace("Bearer ", "");
                var authAdmin = await _auhtServices.validarAdmin(token);
                var authEstudent = await _auhtServices.validarEstudent(token);


                if (!authAdmin.IsSuccessStatusCode && !authEstudent.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "ACCESO DENEGADO" });
                }
                var estudiante = await _context.Estudiantes
                    .Include(e => e.Matriculas).ThenInclude(m => m.Grupo).ThenInclude(g => g.asignatura) // Incluye las matrículas relacionadas
                    .Include(e => e.Matriculas).ThenInclude(m => m.Grupo).ThenInclude(g => g.Horarios)
                    .Include(e => e.Asistencias)
                    .FirstOrDefaultAsync(e => e.Identificacion == id); // Busca el estudiante por identificación

                if (estudiante == null)
                {
                    return NotFound($"No se encontró un estudiante con la identificación {id}.");
                }

                var estudianteDTO = new EstudianteDTO
                {
                    Identificacion = estudiante?.Identificacion,
                    Nombres = estudiante?.Nombres,
                    Apellidos = estudiante?.Apellidos,
                    Foto = estudiante?.Foto,
                    Carrera = estudiante?.Carrera,
                    Matriculas = estudiante?.Matriculas?.Select(m => new MatriculaDTO
                    {
                        FechaMatricula = m?.fechaMatricula,
                        GrupoidGrupo = m?.GrupoidGrupo,
                        grupo = m?.Grupo != null ? new GrupoDTO
                        {
                            IdGrupo = m.Grupo.idGrupo,
                            Descripcion = m.Grupo.Descripcion,
                            asignatura = m.Grupo.asignatura,
                            Horarios = m.Grupo.Horarios?.Select(c => new HorarioDTO
                            {
                                idHorario = c?.idHorario,
                                diaSemana = c?.diaSemana,
                                horaInicio = c?.horaInicio,
                                minutosInicio = c?.minutosInicio,
                                horaFin = c?.horaFin,
                                minutosFin = c?.minutosFin
                            }).ToList()
                        } : null,
                    }).ToList(),
                    Asistencias = estudiante?.Asistencias?.Select(m => new AsistenciaDTO
                    {
                        Fecha = m?.Fecha,
                        Estado = m?.Estado,
                        clase = m?.Clase != null ? new ClaseDTO
                        {
                            IdClase = m.Clase.idClase,
                            Fecha = m.Clase.fecha,
                            Grupo = m.Clase.grupo != null ? new GrupoDTO
                            {
                                IdGrupo = m.Clase.grupo.idGrupo,
                                Descripcion = m.Clase.grupo.Descripcion,
                                asignatura = m.Clase.grupo.asignatura
                            } : null,
                        } : null,
                    }).ToList()
                };


                return Ok(estudianteDTO);
            }
            catch (Exception ex)
            {
                // Manejo de cualquier excepción general
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener el estudiante: {ex.Message}");
            }
        }


        // PUT: api/Estudiantes/Editar/5
        [HttpPut("Editar/{id}")]
        public async Task<ActionResult> EditarEstudiante(string id, EstudianteCreateDTO estudianteEditado)
        {

            if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                return Unauthorized("Token no proporcionado");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (id != estudianteEditado.Identificacion)
                {
                    return BadRequest("El ID del estudiante no coincide con el de la URL.");
                }
                var token = tokenHeader.ToString().Replace("Bearer ", "");
                var authAdmin = await _auhtServices.validarAdmin(token);
                var authEstudent = await _auhtServices.validarEstudent(token);


                if (!authAdmin.IsSuccessStatusCode || !authEstudent.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "ACCESO DENEGADO" });
                }
                var estudianteExistente = await _context.Estudiantes
                    .FirstOrDefaultAsync(e => e.Identificacion == id);

                if (estudianteExistente == null)
                {
                    return NotFound($"No se encontró un estudiante con la identificación {id}.");
                }

                // Actualiza los datos del estudiante existente
                estudianteExistente.Nombres = estudianteEditado.Nombres;
                estudianteExistente.Apellidos = estudianteEditado.Apellidos;
                estudianteExistente.Foto = estudianteEditado.Foto;
                estudianteExistente.Carrera = estudianteEditado.Carrera;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                // Devuelve un mensaje de éxito
                return StatusCode(StatusCodes.Status200OK, new { message = "Estudiante editado con éxito", response = estudianteExistente });
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error de concurrencia al actualizar el estudiante: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar el estudiante: {ex.Message}");
            }
        }


        // DELETE: api/Estudiantes/Eliminar/5
        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult> EliminarEstudiante(string id)
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
                var resultadoApiExterna = await _auhtServices.validarAdmin(token);
                if (!resultadoApiExterna.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "ACCESO DENEGADO" });
                }

                var estudiante = await _context.Estudiantes
                    .Include(e => e.Matriculas)
                    .Include(e => e.Asistencias)
                    .Include(e => e.user)
                    .FirstOrDefaultAsync(e => e.Identificacion == id);

                if (estudiante == null)
                {
                    return NotFound($"No se encontró un estudiante con la identificación {id}.");
                }

                // Buscar el User relacionado usando la propiedad de navegación
                var user = estudiante.user;

                // Si existe un User relacionado, eliminarlo
                if (user != null)
                {
                    _context.Users.Remove(user);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Error al eliminar el estudiante: No encontrado");

                }


                _context.Estudiantes.Remove(estudiante);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                // Devuelve un mensaje de éxito
                return StatusCode(StatusCodes.Status200OK, new { message = "Estudiante eliminado con éxito" });
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al eliminar el estudiante: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al procesar la solicitud: {ex.Message}");
            }
        }

        private bool EstudianteExists(string id)
        {
            return _context.Estudiantes.Any(e => e.Identificacion == id);
        }
    }
}
