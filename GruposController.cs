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
    public class GruposController : ControllerBase
    {
        private readonly Plataforma_Context _context;
        private readonly IAuthServices _authServices ;

        public GruposController(Plataforma_Context context, IAuthServices authServices)
        {
            _context = context;
            _authServices = authServices;  
        }

        // GET: api/Grupos/Listar
        [HttpGet]
        [Route("Listar")]
        public async Task<ActionResult<IEnumerable<GrupoDTO>>> GetGrupos()
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
                var grupos = await _context.Grupos
                    .Include(g => g.Clases)
                    .Include(g => g.Horarios)
                    .Include(g => g.profesor)
                    .Include(g => g.asignatura)  //Incluye los Horarios y clases relacionados
                    .Select(p => new GrupoDTO
                    {
                        IdGrupo = p.idGrupo,
                        Descripcion = p.Descripcion,
                        
                        asignatura=p.asignatura,
                        profesor = p.profesor,

                        Matriculas = p.Matriculas.Select(m => new MatriculaDTO
                        {
                            FechaMatricula = m.fechaMatricula,
                            GrupoidGrupo =m.GrupoidGrupo,
                            estudiante = new EstudianteDTO
                            {
                                Identificacion = m.Estudiante.Identificacion,
                                Nombres = m.Estudiante.Nombres,
                                Apellidos = m.Estudiante.Apellidos,
                                Foto = m.Estudiante.Foto,
                                Carrera = m.Estudiante.Carrera,
                            },


                        }).ToList(),

                        Clases = p.Clases.Select(c => new ClaseDTO
                        {
                            IdClase = c.idClase,
                            Fecha = c.fecha,


                        }).ToList(),
                        
                        
                        Horarios = p.Horarios.Select(c => new HorarioDTO
                        {
                            idHorario = c.idHorario,
                            diaSemana = c.diaSemana,
                            horaInicio = c.horaInicio,
                            minutosInicio = c.minutosInicio,
                            horaFin = c.horaFin,
                            minutosFin = c.minutosFin


                        }).ToList()
                    }).ToListAsync();

                if (grupos == null || !grupos.Any())
                {
                    return NotFound("No se encontraron Grupos.");
                }

                // Devuelve un HTTP 200 (OK) con la lista de profesores
                return Ok(grupos);
            }
            catch (Exception ex)
            {
                // Manejo de cualquier excepción general
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener los profesores: {ex.Message}");
            }
        }

        // GET: api/Grupos/listar/5
        [HttpGet("ListarId/{idGrupo}")]
        public async Task<ActionResult<IEnumerable<GrupoDTO>>> GetGruposId(int idGrupo)
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
                var grupos = await _context.Grupos
                    .Include(g => g.Clases)
                    .Include(g => g.Horarios)
                    .Include(g => g.profesor)
                    .Include(g => g.Matriculas)
                    .Include(g => g.asignatura)  //Incluye los Horarios y clases relacionados
                    .Where(g => g.idGrupo == idGrupo)       // Filtra por idGrupo
                    .Select(p => new GrupoDTO
                    {
                        IdGrupo = p.idGrupo,
                        Descripcion = p.Descripcion,
                        asignatura = p.asignatura,
                        profesor = p.profesor,

                        Matriculas = p.Matriculas.Select(m => new MatriculaDTO
                        {
                            FechaMatricula = m.fechaMatricula,
                            GrupoidGrupo = m.GrupoidGrupo,
                            estudiante = new EstudianteDTO
                            {
                                Identificacion = m.Estudiante.Identificacion,
                                Nombres = m.Estudiante.Nombres,
                                Apellidos = m.Estudiante.Apellidos,
                                Foto = m.Estudiante.Foto,
                                Carrera = m.Estudiante.Carrera,
                            },


                        }).ToList(),

                        Clases = p.Clases.Select(c => new ClaseDTO
                        {
                            IdClase = c.idClase,
                            Fecha = c.fecha,


                        }).ToList(),
                        
                        Horarios = p.Horarios.Select(c => new HorarioDTO
                        {
                            idHorario = c.idHorario,
                            diaSemana = c.diaSemana,
                            horaInicio = c.horaInicio,
                            minutosInicio = c.minutosInicio,
                            horaFin = c.horaFin,
                            minutosFin = c.minutosFin,


                        }).ToList()


                    }).ToListAsync();

                if (grupos == null || !grupos.Any())
                {
                    return NotFound("No se encontró Grupo.");
                }

                // Devuelve un HTTP 200 (OK) con el grupo del id
                return Ok(grupos);
            }
            catch (Exception ex)
            {
                // Manejo de cualquier excepción general
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener los profesores: {ex.Message}");
            }
        }

        //GET api/Grupos/listarProfesot/1003376120
        [HttpGet("ListarProfesor/{idProfesor}")]
        public async Task<ActionResult<IEnumerable<GrupoDTO>>> GetGruposByProfesor(string idProfesor)
        {
            if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                return Unauthorized("Token no proporcionado");
            }
            try
            {
                var token = tokenHeader.ToString().Replace("Bearer ", "");
                var authAdmin = await _authServices.validarAdmin(token);
                var authProfesor = await _authServices.validarProfesor(token);
                if (!authAdmin.IsSuccessStatusCode && !authProfesor.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "ACCESO DENEGADO" });
                }

                    // Filtrar por profesor
                    var grupos = await _context.Grupos
                    .Include(g => g.Horarios)
                    .Include(g => g.profesor)
                    .Include(g => g.asignatura)
                    .Where(g => g.ProfesorIdentificacion == idProfesor)  // Filtra por idProfesor
                    .Select(p => new GrupoDTO
                    {
                        IdGrupo = p.idGrupo,
                        Descripcion = p.Descripcion,
                        profesor = p.profesor,
                        asignatura = p.asignatura,

                        Horarios = p.Horarios.Select(c => new HorarioDTO
                        {
                            idHorario = c.idHorario,
                            diaSemana = c.diaSemana,
                            horaInicio = c.horaInicio,
                            minutosInicio = c.minutosInicio,
                            horaFin = c.horaFin,
                            minutosFin = c.minutosFin,
                        }).ToList()

                    }).ToListAsync();

                if (grupos == null || !grupos.Any())
                {
                    return NotFound("No se encontraron grupos para el profesor.");
                }

                return Ok(grupos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener los grupos: {ex.Message}");
            }
        }



        // POST: api/Grupos/Guardar
        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> PostGrupo(GrupoCreateDTO grupoDto)
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
                // Crear una nueva instancia del modelo Grupo basado en el DTO
                var nuevoGrupo = new Grupo
                {
                    Descripcion = grupoDto.Descripcion,
                    ProfesorIdentificacion = grupoDto.profesorIdentificacion,
                    AsignaturaCodigo = grupoDto.AsignaturaCodigo
                    // No se incluye Clases ni Horarios en el POST ya que no se espera que sean enviados al crear el grupo.
                };

                // Agregar el nuevo grupo a la base de datos
                _context.Grupos.Add(nuevoGrupo);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Devolver un StatusCode 201 Created con el nuevo grupo en la respuesta
                return StatusCode(StatusCodes.Status201Created, new { message = "Grupo creado correctamente.", response = nuevoGrupo });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                // Manejo de cualquier excepción
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al crear el grupo: {ex.Message}");
            }
        }


        // PUT: api/Grupos/Editar/5
        [HttpPut("Editar/{idGrupo}")]
        public async Task<IActionResult> PutGrupo(int idGrupo, GrupoCreateDTO grupoNewDto)
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
                if (idGrupo != grupoNewDto.IdGrupo)
                {
                    return BadRequest("El ID del grupo no coincide con el de la URL.");
                }

                var grupoExistente = await _context.Grupos.FindAsync(idGrupo);

                if (grupoExistente == null)
                {
                    return NotFound($"No se encontró un grupo con el ID {idGrupo}.");
                }

                // Actualizar las propiedades del grupo existente con los valores del DTO
                grupoExistente.Descripcion = grupoNewDto.Descripcion;
                grupoExistente.ProfesorIdentificacion = grupoNewDto.profesorIdentificacion;
                grupoExistente.AsignaturaCodigo = grupoNewDto.AsignaturaCodigo;

                    await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { message = "Grupo actualizado correctamente.", response = grupoExistente });
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();


                if (!GrupoExists(idGrupo))
                {
                    return NotFound($"No se encontró un grupo con el ID {idGrupo}.");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar el grupo.");
                }
            }
        }


        // DELETE: api/Grupos/5
        [HttpDelete("Eliminar/{idGrupo}")]
        public async Task<IActionResult> DeleteGrupo(int idGrupo)
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
                var grupo = await _context.Grupos.FindAsync(idGrupo);

                if (grupo == null)
                {
                    return NotFound($"No se encontró un grupo con el ID {idGrupo}.");
                }

                _context.Grupos.Remove(grupo);


                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { message = "Grupo eliminado correctamente." });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al eliminar el grupo: {ex.Message}");
            }
        }

        private bool GrupoExists(int id)
        {
            return _context.Grupos.Any(e => e.idGrupo == id);
        }
    }
}
