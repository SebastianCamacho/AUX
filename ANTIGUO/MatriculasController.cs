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
    public class MatriculasController : ControllerBase
    {
        private readonly Plataforma_Context _context;
        private readonly IAuthServices _authServices;

        public MatriculasController(Plataforma_Context context, IAuthServices authServices)
        {
            _context = context;
            _authServices = authServices;
        }

        // GET: api/Matriculas/Listar
        [HttpGet]
        [Route("Listar")]
        public async Task<ActionResult<IEnumerable<MatriculaDTO>>> GetMatriculas()
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
                if (!authAdmin.IsSuccessStatusCode && !authStudent.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "ACCESO DENEGADO" });
                }
                var matriculas = await _context.Matriculas
                    .Include(m => m.Estudiante) // Incluye el estudiante relacionado
                    .Include(m => m.Grupo) // Incluye el grupo relacionado
                    .Select(m => new MatriculaDTO
                    {
                        FechaMatricula = m.fechaMatricula,
                        EstudianteIdentificacion = m.EstudianteIdentificacion,
                        GrupoidGrupo = m.GrupoidGrupo,
                        estudiante = new EstudianteDTO
                        {
                            Identificacion = m.Estudiante.Identificacion,
                            Nombres = m.Estudiante.Nombres,
                            Apellidos = m.Estudiante.Apellidos,
                            Foto = m.Estudiante.Foto,
                        },
                        grupo = new GrupoDTO
                        {
                            IdGrupo = m.Grupo.idGrupo,
                            Descripcion = m.Grupo.Descripcion,
                            asignatura = m.Grupo.asignatura
                        }
                    })
                    .ToListAsync();

                if (matriculas == null || !matriculas.Any())
                {
                    return NotFound("No se encontraron matrículas.");
                }

                // Devuelve un HTTP 200 (OK) con la lista de matrículas
                return Ok(matriculas);
            }
            catch (Exception ex)
            {
                // Manejo de cualquier excepción general
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener las matrículas: {ex.Message}");
            }
        }


        // GET: api/Matriculas/ListarId/{estudianteId}/{grupoId}
        [HttpGet("ListarId/{estudianteId}/{grupoId}")]
        public async Task<ActionResult<MatriculaDTO>> GetMatricula(string estudianteId, int grupoId)
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
                if (!authAdmin.IsSuccessStatusCode && !authStudent.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "ACCESO DENEGADO" });
                }
                var matricula = await _context.Matriculas
                    .Include(m => m.Estudiante) // Incluye el estudiante relacionado
                    .Include(m => m.Grupo) // Incluye el grupo relacionado
                    .FirstOrDefaultAsync(m => m.EstudianteIdentificacion == estudianteId && m.GrupoidGrupo == grupoId); // Busca la matrícula por identificación del estudiante y grupo

                if (matricula == null)
                {
                    return NotFound($"No se encontró una matrícula para el estudiante con la identificación {estudianteId} y grupo {grupoId}.");
                }

                var matriculaDTO = new MatriculaDTO
                {
                    FechaMatricula = matricula.fechaMatricula,
                    EstudianteIdentificacion = matricula.EstudianteIdentificacion,
                    GrupoidGrupo = matricula.GrupoidGrupo,
                    estudiante = new EstudianteDTO
                    {
                        Identificacion = matricula.Estudiante.Identificacion,
                        Nombres = matricula.Estudiante.Nombres,
                        Apellidos = matricula.Estudiante.Apellidos,
                        Foto = matricula.Estudiante.Foto,
                    },
                    grupo = new GrupoDTO
                    {
                        IdGrupo = matricula.Grupo.idGrupo,
                        Descripcion = matricula.Grupo.Descripcion,
                        asignatura = matricula.Grupo.asignatura
                    }
                };

                return Ok(matriculaDTO);
            }
            catch (Exception ex)
            {
                // Manejo de cualquier excepción general
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener la matrícula: {ex.Message}");
            }
        }

        // GET: api/Matriculas/ListarId/{estudianteId}/{grupoId}
        [HttpGet("ListarGrupoId/{grupoId}")]
        public async Task<ActionResult<IEnumerable<MatriculaDTO>>> GetMatriculaIdGrupo(int grupoId)
        {
            if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                return Unauthorized("Token no proporcionado");
            }
            try
            {
                var token = tokenHeader.ToString().Replace("Bearer ", "");
                var matriculas = await _context.Matriculas
    .Include(m => m.Estudiante) // Incluye el estudiante relacionado
    .Include(m => m.Grupo) // Incluye el grupo relacionado
    .Where(m => m.GrupoidGrupo == grupoId) // Filtra por el grupoId
    .Select(m => new MatriculaDTO
    {
        FechaMatricula = m.fechaMatricula,
        EstudianteIdentificacion = m.EstudianteIdentificacion,
        GrupoidGrupo = m.GrupoidGrupo,
        estudiante = new EstudianteDTO
        {
            Identificacion = m.Estudiante.Identificacion,
            Nombres = m.Estudiante.Nombres,
            Apellidos = m.Estudiante.Apellidos,
            Foto = m.Estudiante.Foto,
        },
        grupo = new GrupoDTO
        {
            IdGrupo = m.Grupo.idGrupo,
            Descripcion = m.Grupo.Descripcion,
            asignatura = m.Grupo.asignatura
        }
    })
    .ToListAsync(); // Obtén todas las coincidencias en una lista

                if (matriculas == null || !matriculas.Any())
                {
                    return NotFound("No se encontraron matrículas.");
                }
                return Ok(matriculas);
            }
            catch (Exception ex)
            {
                // Manejo de cualquier excepción general
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener la matrícula: {ex.Message}");
            }
        }


        // GET: api/Matriculas/ListarPorEstudiante/{estudianteId}
        [HttpGet("ListarPorEstudiante/{estudianteId}")]
        public async Task<ActionResult<IEnumerable<MatriculaDTO>>> GetMatriculasPorEstudiante(string estudianteId)
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
                if (!authAdmin.IsSuccessStatusCode && !authStudent.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "ACCESO DENEGADO" });
                }
                var matriculas = await _context.Matriculas
                    .Include(m => m.Estudiante) // Incluye el estudiante relacionado
                    .Include(m => m.Grupo).ThenInclude(g => g.Horarios) // Incluye el grupo relacionado
                    .Include(m => m.Grupo).ThenInclude(g => g.asignatura) // Incluye la asignatura relacionada

                    .Where(m => m.EstudianteIdentificacion == estudianteId) // Filtra por identificación del estudiante
                    .ToListAsync();

                if (matriculas == null || !matriculas.Any())
                {
                    return NotFound($"No se encontraron matrículas para el estudiante con la identificación {estudianteId}.");
                }

                var matriculaDTOs = matriculas.Select(matricula => new MatriculaDTO
                {
                    FechaMatricula = matricula.fechaMatricula,
                    EstudianteIdentificacion = matricula.EstudianteIdentificacion,
                    GrupoidGrupo = matricula.GrupoidGrupo,

                    grupo = new GrupoDTO
                    {
                        IdGrupo = matricula.Grupo.idGrupo,
                        Descripcion = matricula.Grupo.Descripcion,
                        asignatura = matricula.Grupo.asignatura,
                        Horarios = matricula.Grupo.Horarios.Select(c => new HorarioDTO
                        {
                            idHorario = c.idHorario,
                            diaSemana = c.diaSemana,
                            horaInicio = c.horaInicio,
                            minutosInicio = c.minutosInicio,
                            horaFin = c.horaFin,
                            minutosFin = c.minutosFin,


                        }).ToList()
                    }

                }).ToList();

                return Ok(matriculaDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener las matrículas: {ex.Message}");
            }
        }

        // POST: api/Matriculas/Guardar
        [HttpPost]
        [Route("Guardar")]
        public async Task<ActionResult> CreateMatricula(MatriculaCreateDTO matriculaCreateDTO)
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
                if (!authAdmin.IsSuccessStatusCode && !authStudent.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "ACCESO DENEGADO" });
                }
                // Verificar si el estudiante ya está matriculado en el grupo
                var existeMatricula = await _context.Matriculas
                    .AnyAsync(m => m.EstudianteIdentificacion == matriculaCreateDTO.EstudianteIdentificacion
                                && m.GrupoidGrupo == matriculaCreateDTO.GrupoidGrupo);

                if (existeMatricula)
                {
                    return BadRequest(new { message = "El estudiante ya está matriculado en este grupo." });
                }

                //Extraer codigo Asignatura
                var codigoAsignatura = await _context.Grupos
                    .Where(g => g.idGrupo == matriculaCreateDTO.GrupoidGrupo)
                    .Select(g => g.asignatura.Codigo)
                    .FirstOrDefaultAsync();

                if (await EstaMatriculadoEnAsignatura(matriculaCreateDTO.EstudianteIdentificacion, codigoAsignatura))
                {
                    return BadRequest(new { message = "El estudiante ya está matriculado en esta asignatura." });
                }

                if (!await ValidarHorarios(matriculaCreateDTO.EstudianteIdentificacion, matriculaCreateDTO.GrupoidGrupo))
                {
                    return BadRequest(new { message = "Cruce de Horarios, Revise el Horario" });
                }


                var matricula = new Matricula
                {
                    fechaMatricula = matriculaCreateDTO.FechaMatricula,
                    EstudianteIdentificacion = matriculaCreateDTO.EstudianteIdentificacion,
                    GrupoidGrupo = matriculaCreateDTO.GrupoidGrupo
                };


                _context.Matriculas.Add(matricula);
                await _context.SaveChangesAsync(); 
                await transaction.CommitAsync();

                // Devuelve un HTTP 201 (Created) con un mensaje personalizado y el recurso creado
                return StatusCode(StatusCodes.Status201Created, new { message = "Matrícula creada con éxito", response = matricula });
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();

                string mensaje = VerificarErrorSql(dbEx.InnerException.Message);
                // Manejo específico para errores de base de datos
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al guardar en la base de datos: {mensaje}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                // Manejo de cualquier otra excepción general
                return StatusCode(StatusCodes.Status400BadRequest, $"Error al procesar la solicitud: {ex.Message}");
            }
        }
        private string VerificarErrorSql(string mensaje)
        {
            // Verificar si el mensaje contiene el texto específico de la excepción
            if (mensaje.Contains("The INSERT statement conflicted with the FOREIGN KEY constraint \"FK_Matriculas_Usuarios_EstudianteIdentificacion\""))
            {
                return "No existe un estudiante con esa identificación.";
            }
            else if (mensaje.Contains("The INSERT statement conflicted with the FOREIGN KEY constraint \"FK_Matriculas_Grupos_GrupoidGrupo\""))
            {
                return "No existe un Grupo con ese id";

            }

            // En caso de que no coincida, devolver el mensaje original o un mensaje genérico
            return mensaje;
        }
        private async Task<bool> EstaMatriculadoEnAsignatura(string estudianteId, string asignaturaId)
        {


            // Verificar si el estudiante tiene alguna matrícula en un grupo de la asignatura dada
            var estaMatriculado = await _context.Matriculas
                .Include(m => m.Grupo)
                .AnyAsync(m => m.EstudianteIdentificacion == estudianteId && m.Grupo.AsignaturaCodigo == asignaturaId);

            return estaMatriculado;

        }
        private async Task<bool> ValidarHorarios(string estudianteIdentificacion, int grupoidGrupo)
        {
            // Obtener los horarios del grupo que se desea matricular
            var grupoNuevo = await _context.Grupos
                .Include(g => g.Horarios) // Incluir horarios del nuevo grupo
                .FirstOrDefaultAsync(g => g.idGrupo == grupoidGrupo);

            if (grupoNuevo == null)
            {
                throw new Exception("Grupo no encontrado.");
            }

            // Obtener los grupos en los que el estudiante ya está matriculado
            var gruposMatriculados = await _context.Matriculas
                .Include(m => m.Grupo).ThenInclude(g => g.Horarios) // Incluir el grupo de la matrícula
                .Where(m => m.EstudianteIdentificacion == estudianteIdentificacion)
                .ToListAsync();

            foreach (var matricula in gruposMatriculados)
            {
                foreach (var horarioNuevo in grupoNuevo.Horarios)
                {
                    // Verificar si hay conflicto de horarios
                    var horariosExistentes = matricula.Grupo.Horarios;
                    foreach (var horarioExistente in horariosExistentes)
                    {
                        // Comprobar si los días son los mismos y si las horas se superponen
                        if (horarioNuevo.diaSemana == horarioExistente.diaSemana &&
                            (horarioNuevo.horaInicio < horarioExistente.horaFin && horarioNuevo.horaFin > horarioExistente.horaInicio ||
                             (horarioNuevo.horaInicio == horarioExistente.horaFin && horarioNuevo.minutosInicio < horarioExistente.minutosFin) ||
                             (horarioNuevo.horaFin == horarioExistente.horaInicio && horarioNuevo.minutosFin > horarioExistente.minutosInicio)))
                        {
                            return false; // Hay un conflicto
                        }
                    }
                }
            }

            return true; // No hay conflictos
        }



        // PUT: api/Matriculas/Editar/{estudianteId}/{grupoId}
        [HttpPut("Editar/{estudianteId}/{grupoId}")]
        public async Task<ActionResult> EditarMatricula(string estudianteId, int grupoId, MatriculaCreateDTO matriculaEditada)
        {


            if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                return Unauthorized("Token no proporcionado");
            }
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {

                if (estudianteId != matriculaEditada.EstudianteIdentificacion)
                {
                    return BadRequest("El ID del no coinciden con el de la URL.");
                }

                var token = tokenHeader.ToString().Replace("Bearer ", "");
                var authAdmin = await _authServices.validarAdmin(token);
                var authStudent = await _authServices.validarEstudent(token);
                if (!authAdmin.IsSuccessStatusCode && !authStudent.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "ACCESO DENEGADO" });
                }
                var matriculaExistente = await _context.Matriculas
                    .Include(m => m.Estudiante)
                    .Include(m => m.Grupo)
                    .FirstOrDefaultAsync(m => m.EstudianteIdentificacion == estudianteId && m.GrupoidGrupo == grupoId);

                if (matriculaExistente == null)
                {
                    return NotFound($"El Estudiante con id {estudianteId} No se encuentra matriculado en el grupo de id {grupoId}");
                }

                // Actualiza los datos de la matrícula existente
                matriculaExistente.fechaMatricula = matriculaEditada.FechaMatricula;
                matriculaExistente.GrupoidGrupo = matriculaEditada.GrupoidGrupo;

                var existeMatricula = await _context.Matriculas
                    .AnyAsync(m => m.EstudianteIdentificacion == matriculaEditada.EstudianteIdentificacion
                                && m.GrupoidGrupo == matriculaEditada.GrupoidGrupo);

                if (existeMatricula)
                {
                    return BadRequest(new { message = "El estudiante ya está matriculado en este grupo." });
                }

                //Extraer codigo Asignatura
                var codigoAsignatura = await _context.Grupos
                    .Where(g => g.idGrupo == matriculaEditada.GrupoidGrupo)
                    .Select(g => g.asignatura.Codigo)
                    .FirstOrDefaultAsync();

                if (await EstaMatriculadoEnAsignatura(matriculaEditada.EstudianteIdentificacion, codigoAsignatura))
                {
                    return BadRequest(new { message = "El estudiante ya está matriculado en esta asignatura." });
                }

                if (!await ValidarHorarios(matriculaEditada.EstudianteIdentificacion, matriculaEditada.GrupoidGrupo))
                {
                    return BadRequest(new { message = "Cruce de Horarios, Revise el Horario" });
                }






                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Devuelve un mensaje de éxito
                return StatusCode(StatusCodes.Status200OK, new { message = "Matrícula editada con éxito", response = matriculaExistente });
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                await transaction.RollbackAsync();

                return StatusCode(StatusCodes.Status500InternalServerError, $"Error de concurrencia al actualizar la matrícula: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar la matrícula: {ex.Message}");
            }
        }

        // DELETE: api/Matriculas/Eliminar/{estudianteId}/{grupoId}
        [HttpDelete("Eliminar/{estudianteId}/{grupoId}")]
        public async Task<ActionResult> EliminarMatricula(string estudianteId, int grupoId)
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
                if (!authAdmin.IsSuccessStatusCode && !authStudent.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "ACCESO DENEGADO" });
                }
                var matricula = await _context.Matriculas
                    .Include(m => m.Estudiante)
                    .Include(m => m.Grupo)
                    .FirstOrDefaultAsync(m => m.EstudianteIdentificacion == estudianteId && m.GrupoidGrupo == grupoId);

                if (matricula == null)
                {
                    return NotFound($"No se encontró una matrícula para el estudiante con la identificación {estudianteId} y grupo {grupoId}.");
                }

                _context.Matriculas.Remove(matricula);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Devuelve un mensaje de éxito
                return StatusCode(StatusCodes.Status200OK, new { message = "Matrícula eliminada con éxito" });
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();

                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al eliminar la matrícula: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al procesar la solicitud: {ex.Message}");
            }
        }


        private bool MatriculaExists(string id)
        {
            return _context.Matriculas.Any(e => e.EstudianteIdentificacion == id);
        }
    }
}
