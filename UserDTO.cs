using ENTITY;

namespace Backend_Asistencias.DTOs
{

    public class UserDTO
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Rol { get; set; }
        public UsuarioCreateDTO? Usuario { get; set; } // UsuarioDTO como base
    }

    

}
