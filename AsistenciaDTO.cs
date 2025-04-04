namespace Backend_Asistencias.DTOs
{
    public class AsistenciaDTO
    {
        public DateTime? Fecha { get; set; }
        public bool? Estado { get; set; }
        public string? EstudianteIdentificacion { get; set; }
        public EstudianteDTO? estudiante { get; set; }
        public string? idClase { get; set; }
        public ClaseDTO? clase { get; set; }

    }
}
