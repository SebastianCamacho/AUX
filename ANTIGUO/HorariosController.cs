using Backend_Asistencias.DTOs;
using Backend_Asistencias.Services;
using ENTITY;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_Asistencias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorariosController : ControllerBase
    {
        private readonly Plataforma_Context _context;
        private readonly IAuthServices _authServices;

        public HorariosController(Plataforma_Context context, IAuthServices authServices)
        {
            _context = context;
            _authServices = authServices;
        }

        // GET: api/Horarios/Listar
        [HttpGet]
        [Route("Listar")]
        public async Task<ActionResult<IEnumerable<HorarioDTO>>> GetHorarios()
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
                var horarios = await _context.Horarios
                    .Include(h => h.grupo)
                    .Select(h => new HorarioDTO
                    {
                        idHorario = h.idHorario,
                        diaSemana = h.diaSemana,
                        horaInicio = h.horaInicio,
                        minutosInicio = h.minutosInicio,
                        horaFin = h.horaFin,
                        minutosFin = h.minutosFin,
                        grupo = new GrupoDTO
                        {
                            IdGrupo = h.grupo.idGrupo,
                            Descripcion = h.grupo.Descripcion,
                            profesorIdentificacion = h.grupo.ProfesorIdentificacion,
                            asignatura = h.grupo.asignatura
                        }
                    })
                    .ToListAsync();

                if (!horarios.Any())
                {
                    return NotFound("No se encontraron horarios.");
                }

                return Ok(horarios);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener los horarios: {ex.Message}");
            }
        }


        // GET: api/Horarios/ListarId/5
        [HttpGet("ListarId/{id}")]
        public async Task<ActionResult<HorarioDTO>> GetHorario(int id)
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
                var horario = await _context.Horarios
                    .Include(h => h.grupo)
                    .Where(h => h.idHorario == id)
                    .Select(h => new HorarioDTO
                    {
                        idHorario = h.idHorario,
                        diaSemana = h.diaSemana,
                        horaInicio = h.horaInicio,
                        minutosInicio = h.minutosInicio,
                        horaFin = h.horaFin,
                        minutosFin = h.minutosFin,
                        grupo = new GrupoDTO
                        {
                            IdGrupo = h.grupo.idGrupo,
                            Descripcion = h.grupo.Descripcion,
                            profesorIdentificacion = h.grupo.ProfesorIdentificacion,
                            AsignaturaCodigo = h.grupo.AsignaturaCodigo
                        }
                    })
                    .FirstOrDefaultAsync();

                if (horario == null)
                {
                    return NotFound($"No se encontró un horario con el ID {id}.");
                }

                return Ok(horario);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener el horario: {ex.Message}");
            }
        }


        // POST: api/Horarios/Guardar
        [HttpPost]
        [Route("Guardar")]
        public async Task<ActionResult> CrearHorario([FromBody] HorarioCreateDTO horarioDTO)
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
                // Validación de horas y minutos
                if (horarioDTO.horaInicio < 1 || horarioDTO.horaInicio > 24 ||
                    horarioDTO.horaFin < 1 || horarioDTO.horaFin > 24)
                {
                    return BadRequest("La hora debe estar entre 1 y 24.");
                }

                if (horarioDTO.minutosInicio < 0 || horarioDTO.minutosInicio > 59 ||
                    horarioDTO.minutosFin < 0 || horarioDTO.minutosFin > 59)
                {
                    return BadRequest("Los minutos deben estar entre 0 y 59.");
                }

                var nuevoHorario = new Horario
                {
                    diaSemana = horarioDTO.diaSemana.ToLower(),
                    horaInicio = horarioDTO.horaInicio,
                    minutosInicio = horarioDTO.minutosInicio,
                    horaFin = horarioDTO.horaFin,
                    minutosFin = horarioDTO.minutosFin,
                    GrupoidGrupo = horarioDTO.GrupoidGrupo
                };

                _context.Horarios.Add(nuevoHorario);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return StatusCode(StatusCodes.Status201Created, new { message = "Horario creado con éxito", response = nuevoHorario });
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();

                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al guardar el horario en la base de datos: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al crear el horario: {ex.Message}");
            }
        }


        // PUT: api/Horarios/Editar/5
        [HttpPut("Editar/{id}")]
        public async Task<ActionResult> EditarHorario(int id, HorarioCreateDTO horarioDTO)
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
                var horarioExistente = await _context.Horarios
                    .FirstOrDefaultAsync(h => h.idHorario == id);

                if (horarioExistente == null)
                {
                    return NotFound($"No se encontró un horario con el ID {id}.");
                }

                // Validación de horas y minutos
                if (horarioDTO.horaInicio < 1 || horarioDTO.horaInicio > 24 ||
                    horarioDTO.horaFin < 1 || horarioDTO.horaFin > 24)
                {
                    return BadRequest("La hora debe estar entre 1 y 24.");
                }

                if (horarioDTO.minutosInicio < 0 || horarioDTO.minutosInicio > 59 ||
                    horarioDTO.minutosFin < 0 || horarioDTO.minutosFin > 59)
                {
                    return BadRequest("Los minutos deben estar entre 0 y 59.");
                }

                horarioExistente.diaSemana = horarioDTO.diaSemana;
                horarioExistente.horaInicio = horarioDTO.horaInicio;
                horarioExistente.minutosInicio = horarioDTO.minutosInicio;
                horarioExistente.horaFin = horarioDTO.horaFin;
                horarioExistente.minutosFin = horarioDTO.minutosFin;
                horarioExistente.GrupoidGrupo = horarioDTO.GrupoidGrupo;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return StatusCode(StatusCodes.Status200OK, new { message = "Horario actualizado con éxito", response = horarioExistente });
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                await transaction.RollbackAsync();

                return StatusCode(StatusCodes.Status500InternalServerError, $"Error de concurrencia al actualizar el horario: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar el horario: {ex.Message}");
            }
        }




        // DELETE: api/Horarios/Eliminar/5
        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult> EliminarHorario(int id)
        {
            if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                return Unauthorized("Token no proporcionado");
            }
            try
            {
                var token = tokenHeader.ToString().Replace("Bearer ", "");
                var authAdmin = await _authServices.validarAdmin(token);
                if (!authAdmin.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "ACCESO DENEGADO" });
                }
                var horario = await _context.Horarios
                    .FirstOrDefaultAsync(h => h.idHorario == id);

                if (horario == null)
                {
                    return NotFound($"No se encontró un horario con el ID {id}.");
                }

                _context.Horarios.Remove(horario);
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, new { message = "Horario eliminado con éxito" });
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al eliminar el horario: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al procesar la solicitud: {ex.Message}");
            }
        }
    }
}
