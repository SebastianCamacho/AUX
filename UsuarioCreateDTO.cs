namespace Backend_Asistencias.DTOs
{
    public class UsuarioCreateDTO
    {
        public string Identificacion { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Foto { get; set; }
        public string? Profesion { get; set; }
        public string? Carrera{ get; set; }
    }
}