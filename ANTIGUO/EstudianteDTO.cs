using ENTITY;

namespace Backend_Asistencias.DTOs
{
    public class EstudianteDTO : UsuarioDTO
    {
        public string Carrera { get; set; }
        public List<MatriculaDTO> Matriculas { get; set; }
        public List<AsistenciaDTO> Asistencias { get; set; }
    }


}
