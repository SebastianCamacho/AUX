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
    public class AsignaturasController : ControllerBase
    {
        private readonly Plataforma_Context _context;
        private readonly IAuthServices _authServices;


        public AsignaturasController(Plataforma_Context context, IAuthServices authServices)
        {
            _context = context;
            _authServices = authServices;
        }

        // GET: api/Asignaturas
        [HttpGet]
        [Route("Listar")]
        public async Task<ActionResult<IEnumerable<AsignaturaDTO>>> GetAsignaturas()
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
                var asignaturas = await _context.Asignaturas
                    .Include(a => a.Grupos) // Incluye los grupos relacionados
                    .Select(a => new AsignaturaDTO
                    {
                        Codigo = a.Codigo,
                        NombreAsignatura = a.NombreAsignatura,
                        Grupos = a.Grupos.Select(g => new GrupoDTO
                        {
                            IdGrupo = g.idGrupo,
                            Descripcion = g.Descripcion,
                            profesor = g.profesor
                        }).ToList()
                    })
                    .ToListAsync();

                if (!asignaturas.Any())
                {
                    return NotFound("No se encontraron asignaturas.");
                }

                return Ok(asignaturas);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener las asignaturas: {ex.Message}");
            }
        }

        // GET: api/Asignaturas/5
        [HttpGet("ListarCodigo/{codigo}")]
        public async Task<ActionResult<AsignaturaDTO>> GetAsignatura(string codigo)
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
                var asignatura = await _context.Asignaturas
                    .Include(a => a.Grupos) // Incluye los grupos relacionados
                    .Where(a => a.Codigo == codigo)
                    .Select(a => new AsignaturaDTO
                    {
                        Codigo = a.Codigo,
                        NombreAsignatura = a.NombreAsignatura,
                        Grupos = a.Grupos.Select(g => new GrupoDTO
                        {
                            IdGrupo = g.idGrupo,
                            Descripcion = g.Descripcion,
                            profesor = g.profesor
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (asignatura == null)
                {
                    return NotFound($"No se encontró una asignatura con el código {codigo}.");
                }

                return Ok(asignatura);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener la asignatura: {ex.Message}");
            }
        }

        // PUT: api/Asignaturas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Editar/{codigo}")]
        public async Task<ActionResult> EditarAsignatura(string codigo, AsignaturaCreateDTO asignaturaDTO)
        {
            if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                return Unauthorized("Token no proporcionado");
            }

            // Inicia una transacción
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var token = tokenHeader.ToString().Replace("Bearer ", "");
                var authAdmin = await _authServices.validarAdmin(token);
                if (!authAdmin.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "ACCESO DENEGADO" });
                }
                var asignaturaExistente = await _context.Asignaturas
                    .FirstOrDefaultAsync(a => a.Codigo == codigo);

                if (asignaturaExistente == null)
                {
                    return NotFound($"No se encontró una asignatura con el código {codigo}.");
                }
                asignaturaExistente.Codigo = asignaturaDTO.Codigo;
                asignaturaExistente.NombreAsignatura = asignaturaDTO.NombreAsignatura;

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();


                return StatusCode(StatusCodes.Status200OK, new { message = "Asignatura actualizada con éxito", response = asignaturaExistente });
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                await transaction.RollbackAsync();

                return StatusCode(StatusCodes.Status500InternalServerError, $"Error de concurrencia al actualizar la asignatura: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar la asignatura: {ex.Message}");
            }
        }

        // POST: api/Asignaturas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("Guardar")]
        public async Task<ActionResult> CrearAsignatura(AsignaturaCreateDTO asignaturaDTO)
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
                var authStudent = await _authServices.validarEstudent(token);
                var authProfesor = await _authServices.validarProfesor(token);
                if (!authAdmin.IsSuccessStatusCode && !authProfesor.IsSuccessStatusCode && !authStudent.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "ACCESO DENEGADO" });
                }
                var nuevaAsignatura = new Asignatura
                {
                    Codigo = asignaturaDTO.Codigo,
                    NombreAsignatura = asignaturaDTO.NombreAsignatura
                    // No se añaden grupos en la creación
                };

                _context.Asignaturas.Add(nuevaAsignatura);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();


                return StatusCode(StatusCodes.Status201Created, new { message = "Asignatura creada con éxito", response = nuevaAsignatura });
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al guardar la asignatura en la base de datos: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al crear la asignatura: {ex.Message}");
            }
        }

        // DELETE: api/Asignaturas/5
        [HttpDelete("Eliminar/{codigo}")]
        public async Task<ActionResult> EliminarAsignatura(string codigo)
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
                var resultadoApiExterna = await _authServices.validarAdmin(token);
                if (!resultadoApiExterna.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "ACCESO DENEGADO" });
                }
                var asignatura = await _context.Asignaturas
                    .FirstOrDefaultAsync(a => a.Codigo == codigo);

                if (asignatura == null)
                {
                    return NotFound($"No se encontró una asignatura con el código {codigo}.");
                }

                _context.Asignaturas.Remove(asignatura);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return StatusCode(StatusCodes.Status200OK, new { message = "Asignatura eliminada con éxito" });
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();

                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al eliminar la asignatura: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al procesar la solicitud: {ex.Message}");
            }
        }

        private bool AsignaturaExists(string id)
        {
            return _context.Asignaturas.Any(e => e.Codigo == id);
        }
    }
}
