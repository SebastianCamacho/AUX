namespace Backend_Asistencias.DTOs
{
    public class UserRegisterDTO
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? UsuarioIdentificacion { get; set; }
        public string? Rol { get; set; }

        public UserRegisterDTO()
        {
            
        }
        public UserRegisterDTO(UserCreateDTO user)
        {
            Email = user.Email;
            Password = user.Clave;
            UsuarioIdentificacion = user.Usuario.Identificacion;
            Rol = user.Rol;
        }

    }
}
