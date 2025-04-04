namespace Backend_Asistencias.DTOs
{
    public class UserCreateDTO
    {
        public string? Email { get; set; }
        public string? Clave { get; set; }
        public string? Salt { get; set; }
        public string? Rol { get; set; }
        public UsuarioCreateDTO? Usuario { get; set; } // UsuarioDTO como base


    }
}
