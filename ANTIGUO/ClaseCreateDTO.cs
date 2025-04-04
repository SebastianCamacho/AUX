namespace Backend_Asistencias.DTOs
{
    public class ClaseCreateDTO
    {
        public string IdClase { get; set; }
        public DateTime Fecha { get; set; }
        public int GrupoidGrupo { get; set; } // Solo se requiere el ID del grupo
       

        public ClaseCreateDTO() { }
    }
}
