using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ENTITY;
using NuGet.Protocol.Plugins;
using Backend_Asistencias.DTOs;
using Backend_Asistencias.Services;

namespace Backend_Asistencias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfesoresController : ControllerBase
    {
        private readonly Plataforma_Context _context;
        private readonly IAuthServices _authServices;


        public ProfesoresController(Plataforma_Context context, IAuthServices authServices)
        {
            _context = context;
            _authServices = authServices;
        }

        // GET: api/Profesores/Listar
        [HttpGet]
        [Route("Listar")]
        public async Task<ActionResult<IEnumerable<ProfesorDTO>>> GetProfesores()
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
                var profesores = await _context.Profesores
                    .Include(p => p.Grupos).ThenInclude( g => g.asignatura).Include(g => g.Grupos).ThenInclude(g => g.Horarios) //Incluye los grupos relacionados
                    .Select(p => new ProfesorDTO
                    {
                        Identificacion = p.Identificacion,
                        Nombres = p.Nombres,
                        Apellidos = p.Apellidos,
                        Foto = p.Foto,
                        Profesion = p.Profesion,
                        Grupos = p.Grupos.Select(g => new GrupoDTO
                        {
                            IdGrupo = g.idGrupo,
                            Descripcion = g.Descripcion,
                            asignatura = g.asignatura
                            


                        }).ToList()
                    })
                    .ToListAsync();
                    
                if (profesores == null || !profesores.Any())
                {
                    return NotFound("No se encontraron profesores.");
                }

                // Devuelve un HTTP 200 (OK) con la lista de profesores
                return Ok(profesores);
            }
            catch (Exception ex)
            {
                // Manejo de cualquier excepción general
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener los profesores: {ex.Message}");
            }
        }

        // GET: api/Profesores/ListarId/5
        [HttpGet("ListarId/{id}")]
        public async Task<ActionResult<ProfesorDTO>> GetProfesor(string id)
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
                var profesor = await _context.Profesores
                    .Include(p => p.Grupos) // Incluye los grupos relacionados
                    .FirstOrDefaultAsync(p => p.Identificacion == id); // Busca el profesor por identificación

                if (profesor == null)
                {
                    return NotFound($"No se encontró un profesor con la identificación {id}.");
                }

                var profesorDTO = new ProfesorDTO
                {
                    Identificacion = profesor.Identificacion,
                    Nombres = profesor.Nombres,
                    Apellidos = profesor.Apellidos,
                    Foto = profesor.Foto,
                    Profesion = profesor.Profesion,
                    Grupos = profesor.Grupos.Select(g => new GrupoDTO
                    {
                        IdGrupo = g.idGrupo,
                        Descripcion = g.Descripcion,
                        asignatura = g.asignatura


                    }).ToList()
                };

                return Ok(profesorDTO);
            }
            catch (Exception ex)
            {
                // Manejo de cualquier excepción general
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener el profesor: {ex.Message}");
            }
        }
        
        
        

        // PUT: api/Profesores/Editar/5
        [HttpPut("Editar/{id}")]
        public async Task<ActionResult> EditarProfesor(string id, ProfesorCreateDTO profesorEditado)
        {
            
            if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                return Unauthorized("Token no proporcionado");
            }
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (id != profesorEditado.Identificacion)
                {
                    return BadRequest("El ID del profesor no coincide con el de la URL.");
                }

                var token = tokenHeader.ToString().Replace("Bearer ", "");
                var authAdmin = await _authServices.validarAdmin(token);
                var authProfesor = await _authServices.validarProfesor(token);
                if (!authAdmin.IsSuccessStatusCode && !authProfesor.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "ACCESO DENEGADO" });
                }
                var profesorExistente = await _context.Profesores
                    .Include(p => p.Grupos)
                    .FirstOrDefaultAsync(p => p.Identificacion == id);

                if (profesorExistente == null)
                {
                    return NotFound($"No se encontró un profesor con la identificación {id}.");
                }

                // Actualiza los datos del profesor existente
                profesorExistente.Nombres = profesorEditado.Nombres;
                profesorExistente.Apellidos = profesorEditado.Apellidos;
                profesorExistente.Foto = profesorEditado.Foto;
                profesorExistente.Profesion = profesorEditado.Profesion;

                // Si es necesario, también puedes manejar la actualización de los grupos aquí
                // profesorExistente.Grupos = profesorEditado.Grupos;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Devuelve un mensaje de éxito
                return StatusCode(StatusCodes.Status200OK, new { message = "Profesor editado con éxito", response = profesorExistente });
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                await transaction.RollbackAsync();

                return StatusCode(StatusCodes.Status500InternalServerError, $"Error de concurrencia al actualizar el profesor: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar el profesor: {ex.Message}");
            }
        }
       
        // DELETE: api/Profesores/Eliminar/5
        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult> EliminarProfesor(string id)
        {

            // 1. Extraer el token del encabezado 'Authorization'
            if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                return Unauthorized("Token no proporcionado");
            }
            try
            {
                var token = tokenHeader.ToString().Replace("Bearer ", "");
                var resultadoApiExterna = await _authServices.validarAdmin(token);
                if (!resultadoApiExterna.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "ACCESO DENEGADO"});
                }


                var profesor = await _context.Profesores
                    .Include(p => p.Grupos)
                    .Include(p => p.user)
                    .FirstOrDefaultAsync(p => p.Identificacion == id);

                if (profesor == null)
                {
                    return NotFound($"No se encontró un profesor con la identificación {id}.");
                }

                // Buscar el User relacionado usando la propiedad de navegación
                var user = profesor.user;

                // Si existe un User relacionado, eliminarlo
                if (user != null)
                {
                    _context.Users.Remove(user);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Error al eliminar el profesor: No encontrado");

                }

                _context.Profesores.Remove(profesor);
                await _context.SaveChangesAsync();

                // Devuelve un mensaje de éxito
                return StatusCode(StatusCodes.Status200OK, new { message ="Profesor eliminado con éxito" });
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al eliminar el profesor: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al procesar la solicitud: {ex.Message}");
            }
        }

        private bool ProfesorExists(string id)
        {
            return _context.Profesores.Any(e => e.Identificacion == id);
        }
    }
}
