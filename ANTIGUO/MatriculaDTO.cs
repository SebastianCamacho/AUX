using ENTITY;

namespace Backend_Asistencias.DTOs
{
    public class MatriculaDTO
    {
        public DateTime? FechaMatricula { get; set; }
        public string? EstudianteIdentificacion { get; set; }
        public int? GrupoidGrupo { get; set; }
        public EstudianteDTO? estudiante{ get; set; }
        public GrupoDTO? grupo { get; set; }
    }
}