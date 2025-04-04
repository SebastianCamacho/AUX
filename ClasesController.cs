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

namespace Backend_Asistencias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClasesController : ControllerBase
    {
        private readonly Plataforma_Context _context;
        private readonly IAuthServices _authServices;

        public ClasesController(Plataforma_Context context, IAuthServices authServices)
        {
            _context = context;
            _authServices = authServices;
        }
        
        // GET: api/Clases/Listar
        [HttpGet]
        [Route("Listar")]
        public async Task<ActionResult<IEnumerable<ClaseDTO>>> GetClases()
        {
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
                var clases = await _context.Clases
                    .Include(c => c.grupo)
                    .Include(c => c.Asistencias)// Incluye el grupo relacionado
                    .Select(c => new ClaseDTO
                    {
                        IdClase = c.idClase,
                        Fecha = c.fecha,
                        Grupo = new GrupoDTO
                        {
                            IdGrupo = c.grupo.idGrupo,
                            Descripcion = c.grupo.Descripcion,
                            profesorIdentificacion = c.grupo.ProfesorIdentificacion,
                            AsignaturaCodigo = c.grupo.AsignaturaCodigo
                            
                        },
                        Asistencias = c.Asistencias.Select(m => new AsistenciaDTO
                        {
                            Fecha = m.Fecha,
                            Estado = m.Estado,
                            estudiante = new EstudianteDTO
                            {
                                Identificacion = m.Estudiante.Identificacion,
                                Nombres = m.Estudiante.Nombres,
                                Apellidos = m.Estudiante.Apellidos,
                                Foto = m.Estudiante.Foto,
                                Carrera = m.Estudiante.Carrera,
                            },
                        }).ToList()

                    })
                    .ToListAsync();

                if (!clases.Any())
                {
                    return NotFound("No se encontraron clases.");
                }

                return Ok(clases);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener las clases: {ex.Message}");
            }
        }

        // GET: api/Clases/ListarId/5
        [HttpGet("ListarId/{id}")]
        public async Task<ActionResult<ClaseDTO>> GetClase(int id)
        {
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
                var clase = await _context.Clases
                    .Include(c => c.grupo) // Incluye el grupo relacionado
                    .Where(c => c.idClase == id)
                    .Select(c => new ClaseDTO
                    {
                        IdClase = c.idClase,
                        Fecha = c.fecha,
                        Grupo = new GrupoDTO
                        {
                            IdGrupo = c.grupo.idGrupo,
                            Descripcion = c.grupo.Descripcion,
                            profesorIdentificacion = c.grupo.ProfesorIdentificacion,
                            AsignaturaCodigo = c.grupo.AsignaturaCodigo
                        },
                        Asistencias = c.Asistencias.Select(m => new AsistenciaDTO
                        {
                            Fecha = m.Fecha,
                            Estado = m.Estado,
                            estudiante = new EstudianteDTO
                            {
                                Identificacion = m.Estudiante.Identificacion,
                                Nombres = m.Estudiante.Nombres,
                                Apellidos = m.Estudiante.Apellidos,
                                Foto = m.Estudiante.Foto,
                                Carrera = m.Estudiante.Carrera,
                            },
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (clase == null)
                {
                    return NotFound($"No se encontró una clase con el ID {id}.");
                }

                return Ok(clase);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener la clase: {ex.Message}");
            }
        }

        // PUT: api/Clases/Editar/5
        [HttpPut("Editar/{id}")]
        public async Task<ActionResult> EditarClase(int id, ClaseCreateDTO claseDTO)
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
                var authProfesor = await _authServices.validarProfesor(token);
                if (!authAdmin.IsSuccessStatusCode && !authProfesor.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "ACCESO DENEGADO" });
                }
                var claseExistente = await _context.Clases
                    .FirstOrDefaultAsync(c => c.idClase == id);

                if (claseExistente == null)
                {
                    return NotFound($"No se encontró una clase con el ID {id}.");
                }

                claseExistente.fecha = claseDTO.Fecha;
                claseExistente.GrupoidGrupo = claseDTO.GrupoidGrupo;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return StatusCode(StatusCodes.Status200OK, new { message = "Clase actualizada con éxito", response = claseExistente });
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error de concurrencia al actualizar la clase: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar la clase: {ex.Message}");
            }
        }

        // POST: api/Clases/Guardar
        [HttpPost]
        [Route("Guardar")]
        public async Task<ActionResult> CrearClase(ClaseCreateDTO claseDTO)
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
                var authProfesor = await _authServices.validarProfesor(token);
                if (!authAdmin.IsSuccessStatusCode && !authProfesor.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "ACCESO DENEGADO" });
                }
                var grupo = await _context.Grupos.Where(g=>g.idGrupo == claseDTO.GrupoidGrupo).Include(g => g.Horarios).FirstOrDefaultAsync();
                if (grupo == null)
                {
                    return BadRequest("El grupo no existe");
                }
                var horarios = grupo.Horarios;
                if (grupo.Horarios == null || grupo.Horarios.Count == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Error: El grupo no tiene horarios definidos.");

                }
                if (!ValidadorHorario(horarios, claseDTO.Fecha))
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Error: Elija una fecha que concida con el horario del grupo");


                }



                var nuevaClase = new Clase
                {
                    fecha = claseDTO.Fecha,
                    GrupoidGrupo = claseDTO.GrupoidGrupo // Solo se usa el ID del grupo
                };

                _context.Clases.Add(nuevaClase);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return StatusCode(StatusCodes.Status201Created, new { message = "Clase creada con éxito", response = nuevaClase });
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al guardar la clase en la base de datos: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al crear la clase: {ex.Message}");
            }
        }
        private bool ValidadorHorario(List<Horario> horarios, DateTime fechaClase)
        {
            // Obtener el día de la semana de la fecha de la clase
            string diaSemanaClase = fechaClase.ToString("dddd", new System.Globalization.CultureInfo("es-ES"));
            

            // Verificar si el grupo tiene un horario que coincida con el día de la semana de la fecha
            return horarios.Any(h => h.diaSemana.Equals(diaSemanaClase, StringComparison.OrdinalIgnoreCase));
        }



        // DELETE: api/Clases/5
        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult> EliminarClase(int id)
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
                var authProfesor = await _authServices.validarProfesor(token);
                if (!authAdmin.IsSuccessStatusCode && !authProfesor.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "ACCESO DENEGADO" });
                }
                var clase = await _context.Clases
                    .FirstOrDefaultAsync(c => c.idClase == id);

                if (clase == null)
                {
                    return NotFound($"No se encontró una clase con el ID {id}.");
                }

                _context.Clases.Remove(clase);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return StatusCode(StatusCodes.Status200OK, new { message = "Clase eliminada con éxito" });
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al eliminar la clase: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al procesar la solicitud: {ex.Message}");
            }
        }

        
        private bool ClaseExists(int id)
        {
            return _context.Clases.Any(e => e.idClase == id);
        }
    }
}
