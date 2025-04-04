namespace Backend_Asistencias.DTOs
{
    internal class AdministradorCreateDTO : UsuarioCreateDTO
    {
        public string Identificacion { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Foto { get; set; }
    }
}