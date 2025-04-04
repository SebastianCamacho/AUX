using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ENTITY;
using Backend_Asistencias.DTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Internal;
using Backend_Asistencias.Services;

namespace Backend_Asistencias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly INotificacionService _notificacionService;
        private readonly Plataforma_Context _context;
        private readonly IRegisterService _registerService;
        private readonly IAuthServices _authServices;
        





        public UsersController(Plataforma_Context context, IRegisterService registerServive, IAuthServices authServices, INotificacionService notificacionService)
        {
            _context = context;
            _registerService = registerServive;
            _authServices = authServices;
            _notificacionService = notificacionService;

        }

        [HttpGet]
        [Route("Listar")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
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
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "ACCESO DENEGADO" });
                }
                var users = await _context.Users
                 .ToListAsync(); // Aquí traemos los datos a la memoria

                // Luego realizamos la conversión en memoria
                var userDTOs = users.Select(u => new UserDTO
                {
                    Id = u.id,
                    Email = u.email,
                    Rol = u.rol,
                    Usuario = u.Usuario switch
                    {
                        Profesor profesor => new UsuarioCreateDTO
                        {
                            Identificacion = profesor.Identificacion,
                            Nombres = profesor.Nombres,
                            Apellidos = profesor.Apellidos,
                            Foto = profesor.Foto,
                            Profesion = profesor.Profesion
                        },
                        Estudiante estudiante => new UsuarioCreateDTO
                        {
                            Identificacion = estudiante.Identificacion,
                            Nombres = estudiante.Nombres,
                            Apellidos = estudiante.Apellidos,
                            Foto = estudiante.Foto,
                            Carrera = estudiante.Carrera
                        },
                        Administrador Admin => new AdministradorCreateDTO
                        {
                            Identificacion = Admin.Identificacion,
                            Nombres = Admin.Nombres,
                            Apellidos = Admin.Apellidos,
                            Foto = Admin.Foto,
                        },
                        _ => null
                    }
                }).ToList();

                if (users == null || !users.Any())
                {
                    return NotFound("No se encontraron usuarios.");
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener los usuarios: {ex.Message}");

            }
        }

        // GET: api/Users/ListarId/5
        [HttpGet("ListarId/{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
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
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "ACCESO DENEGADO" });
                }
                var user = await _context.Users
                    .Include(u => u.Usuario)
                    .FirstOrDefaultAsync(u => u.id == id);

                if (user == null)
                {
                    return NotFound($"No se encontró un usuario con el ID {id}.");
                }

                // Convertimos el Usuario al DTO adecuado
                UsuarioCreateDTO usuarioDTO = null;
                if (user.Usuario is Profesor profesor)
                {
                    usuarioDTO = new UsuarioCreateDTO
                    {
                        Identificacion = profesor.Identificacion,
                        Nombres = profesor.Nombres,
                        Apellidos = profesor.Apellidos,
                        Foto = profesor.Foto,
                        Profesion = profesor.Profesion
                    };
                }
                else if (user.Usuario is Estudiante estudiante)
                {
                    usuarioDTO = new UsuarioCreateDTO
                    {
                        Identificacion = estudiante.Identificacion,
                        Nombres = estudiante.Nombres,
                        Apellidos = estudiante.Apellidos,
                        Foto = estudiante.Foto,
                        Carrera = estudiante.Carrera
                    };
                }
                else if (user.Usuario is Administrador admin)
                {
                    usuarioDTO = new AdministradorCreateDTO
                    {
                        Identificacion = admin.Identificacion,
                        Nombres = admin.Nombres,
                        Apellidos = admin.Apellidos,
                        Foto = admin.Foto,
                    };
                }

                // Crear el UserDTO con la información del usuario y el UsuarioDTO específico
                var userDTO = new UserDTO
                {
                    Id = user.id,
                    Email = user.email,
                    Rol = user.rol,
                    Usuario = usuarioDTO // Usuario convertido a ProfesorDTO o EstudianteDTO
                };

                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener el usuario: {ex.Message}");
            }
        }

        [HttpGet("BuscarPorEmail/{email}")]
        public async Task<ActionResult<UserDTO>> GetUserByEmail(string email)
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
                var user = await _context.Users
                    .Include(u => u.Usuario)
                    .FirstOrDefaultAsync(u => u.email == email);

                if (user == null)
                {
                    return NotFound($"No se encontró un usuario con el email {email}.");
                }

                // Convertimos el Usuario al DTO adecuado
                UsuarioCreateDTO usuarioDTO = null;
                if (user.Usuario is Profesor profesor)
                {
                    usuarioDTO = new UsuarioCreateDTO
                    {
                        Identificacion = profesor.Identificacion,
                        Nombres = profesor.Nombres,
                        Apellidos = profesor.Apellidos,
                        Foto = profesor.Foto,
                        Profesion = profesor.Profesion
                    };
                }
                else if (user.Usuario is Estudiante estudiante)
                {
                    usuarioDTO = new UsuarioCreateDTO
                    {
                        Identificacion = estudiante.Identificacion,
                        Nombres = estudiante.Nombres,
                        Apellidos = estudiante.Apellidos,
                        Foto = estudiante.Foto,
                        Carrera = estudiante.Carrera
                    };
                }
                else if (user.Usuario is Administrador admin)
                {
                    usuarioDTO = new AdministradorCreateDTO
                    {
                        Identificacion = admin.Identificacion,
                        Nombres = admin.Nombres,
                        Apellidos = admin.Apellidos,
                        Foto = admin.Foto,
                    };
                }

                // Crear el UserDTO con la información del usuario y el UsuarioDTO específico
                var userDTO = new UserDTO
                {
                    Id = user.id,
                    Email = user.email,
                    Rol = user.rol,
                    Usuario = usuarioDTO // Usuario convertido a ProfesorDTO, EstudianteDTO o AdministradorDTO
                };

                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener el usuario: {ex.Message}");
            }
        }


        [HttpPost]
        [Route("Guardar")]
        public async Task<ActionResult> CreateUser(UserCreateDTO userCreateDTO)
        {
            // . Extraer el token del encabezado 'Authorization'
            if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                return Unauthorized("Token no proporcionado");
            }

           
                try
                {
                    var existingUser = await _context.Users
                        .FirstOrDefaultAsync(u => u.email == userCreateDTO.Email);

                    if (existingUser != null)
                    {

                        return Conflict("Ya existe un usuario con el mismo correo electrónico.");
                    }

                    Usuario usuario;

                    if (userCreateDTO.Rol == "Profesor")
                    {
                        usuario = new Profesor
                        {
                            Identificacion = userCreateDTO.Usuario.Identificacion,
                            Nombres = userCreateDTO.Usuario.Nombres,
                            Apellidos = userCreateDTO.Usuario.Apellidos,
                            Foto = userCreateDTO.Usuario.Foto,
                            Profesion = userCreateDTO.Usuario.Profesion
                        };
                    }
                    else if (userCreateDTO.Rol == "Estudiante")
                    {
                        usuario = new Estudiante
                        {
                            Identificacion = userCreateDTO.Usuario.Identificacion,
                            Nombres = userCreateDTO.Usuario.Nombres,
                            Apellidos = userCreateDTO.Usuario.Apellidos,
                            Foto = userCreateDTO.Usuario.Foto,
                            Carrera = userCreateDTO.Usuario.Carrera,
                        };
                    }
                    else if (userCreateDTO.Rol == "Admin")
                    {
                        usuario = new Administrador
                        {
                            Identificacion = userCreateDTO.Usuario.Identificacion,
                            Nombres = userCreateDTO.Usuario.Nombres,
                            Apellidos = userCreateDTO.Usuario.Apellidos,
                            Foto = userCreateDTO.Usuario.Foto,
                        };
                    }
                    else
                    {
                        return BadRequest("Tipo de usuario no válido.");
                    }
                    _context.Usuarios.Add(usuario);
                    await _context.SaveChangesAsync();

                    var user = new User
                    {
                        email = userCreateDTO.Email,
                        clave = userCreateDTO.Clave,
                        salt = userCreateDTO.Salt,
                        rol = userCreateDTO.Rol,
                    };
                    // . Limpiar el token si viene con 'Bearer' en la cabecera
                    var token = tokenHeader.ToString().Replace("Bearer ", "");

                    UserRegisterDTO userRegister = new UserRegisterDTO(userCreateDTO);

                    

                    // . Hacer la solicitud HTTP a la API externa con el token
                    var resultadoApiExterna = await _registerService.Registrar(token, userRegister);

                    

                    if (!resultadoApiExterna.IsSuccessStatusCode)
                    {
                        _context.Usuarios.Remove(usuario);
                        await _context.SaveChangesAsync();
                        return StatusCode(StatusCodes.Status500InternalServerError, "Error: " + resultadoApiExterna);
                    }

                Correo _correo = new Correo
                {
                    ToEmail = user.email,
                    Subject = "USUARIO REGISTRADO",
                    Body = $"Bienvenido {usuario.Nombres} ha sido Registrado en la plataforma estudiantil, " +
                    $"ya puede ingresar con sus nuevas credenciales " +
                    $"\nCorreo: {user.email}\nPassword:{user.clave}"
                };

                string message = await _notificacionService.sendCorreo(_correo);


                   

                    return StatusCode(StatusCodes.Status201Created, new { message = $"Usuario creado con éxito \n{message}", response = user });
                }
                catch (DbUpdateException dbEx)
                {

                    return StatusCode(StatusCodes.Status500InternalServerError, $"Error en la base de datos: {dbEx.Message}");
                }
                catch (Exception ex)
                {

                    return StatusCode(StatusCodes.Status500InternalServerError, $"Error al procesar la solicitud: {ex.Message}");
                }
            }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.id == id);
        }
    }
}
