using ENTITY;

namespace Backend_Asistencias.DTOs
{
    public class GrupoDTO
    {
        public int? IdGrupo { get; set; }
        public string Descripcion { get; set; }
        public string profesorIdentificacion { get; set; }
        public string AsignaturaCodigo { get; set; }

        public Profesor profesor { get; set; }
        public Asignatura asignatura { get; set; }

        public List<ClaseDTO> Clases { get; set; }
        public List<HorarioDTO> Horarios { get; set; }
        public List<MatriculaDTO> Matriculas { get; set; }

    }
}
