namespace Backend_Asistencias.DTOs
{
    public class AsignaturaDTO
    {
        public string Codigo { get; set; }
        public string NombreAsignatura { get; set; }
        public List<GrupoDTO>? Grupos { get; set; } // Incluimos los grupos solo en el GET

        public AsignaturaDTO() { }
    }
}
