using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class Horario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idHorario { get; set; }
        public string diaSemana { get; set; }
        public int horaInicio { get; set; } // Ahora es de tipo entero
        public int minutosInicio { get; set; } // Agregado
        public int horaFin { get; set; } // Ahora es de tipo entero
        public int minutosFin { get; set; }
        public int GrupoidGrupo { get; set; }
        public Grupo? grupo { get; set; }
    }
}
