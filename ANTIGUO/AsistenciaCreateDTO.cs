namespace Backend_Asistencias.DTOs
{
    public class AsistenciaCreateDTO
    {
        public DateTime Fecha { get; set; }
        public bool Estado { get; set; }
        public string EstudianteIdentificacion { get; set; }
        public int idClase { get; set; }
    }
}
