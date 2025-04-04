using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ENTITY
{
    public class Matricula
    {
        public DateTime fechaMatricula { get; set; }

        public string EstudianteIdentificacion { get; set; }
        //PROPIEDAD NAVEGACIONAL
        public Estudiante Estudiante { get; set; }

        public int GrupoidGrupo { get; set; }
        //PROPIEDAD NAVEGACIONAL
        public Grupo Grupo{ get; set; }




        public Matricula()
        {
            
        }

        public Matricula(string estudianteIdentificacion, int grupoidGrupo, Estudiante estudiante, Grupo grupo)
        {
            EstudianteIdentificacion = estudianteIdentificacion;
            GrupoidGrupo = grupoidGrupo;
            Estudiante = estudiante;
            Grupo = grupo;
        }
    }
}
