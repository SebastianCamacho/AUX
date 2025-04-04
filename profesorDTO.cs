namespace Backend_Asistencias.DTOs
{

    public class ProfesorDTO : UsuarioDTO
    {
        public string Profesion { get; set; }
        public List<GrupoDTO> Grupos { get; set; }
    }

}
