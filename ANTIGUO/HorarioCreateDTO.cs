using ENTITY;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Backend_Asistencias.DTOs
{
    public class HorarioCreateDTO
    {
        public int idHorario { get; set; }
        public string diaSemana { get; set; }
        public int horaInicio { get; set; } // Ahora es de tipo entero
        public int minutosInicio { get; set; } // Agregado
        public int horaFin { get; set; } // Ahora es de tipo entero
        public int minutosFin { get; set; }
        public int GrupoidGrupo { get; set; }
    }
}
