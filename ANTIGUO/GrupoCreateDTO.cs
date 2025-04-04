namespace Backend_Asistencias.DTOs
{
    public class GrupoCreateDTO
    {
        
            public int IdGrupo { get; set; }
            public string Descripcion { get; set; }
            public string? profesorIdentificacion { get; set; }
            public string AsignaturaCodigo { get; set; }

        
    }
}
