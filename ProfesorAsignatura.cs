using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class ProfesorAsignatura
    {
        public string ProfesorIdentificacion { get; set; }
        public string AsignaturaCodigo { get; set; }
        public Profesor profesor { get; set; }
        public Asignatura asignatura { get; set; }
    }
}
