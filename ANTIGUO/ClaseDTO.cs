namespace Backend_Asistencias.DTOs
{
    public class ClaseDTO
    {
        public int IdClase { get; set; }
        public DateTime Fecha { get; set; }
        public GrupoDTO Grupo { get; set; } // Incluimos la información del grupo

        public ClaseDTO() { }
        public List<AsistenciaDTO> Asistencias { get; set; }
    }
}