using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class Asistencia
    {
        public DateTime Fecha { get; set; }
        public bool Estado { get; set; }
        // Claves foráneas definidas como propiedades independientes
        public string EstudianteIdentificacion { get; set; }
        public Estudiante Estudiante { get; set; }
        
        public int idClase { get; set; }
        public Clase Clase { get; set; }


        //CONSTRUCTORES
        public Asistencia() {
        
        }

        public Asistencia(DateTime fecha, bool estado, string estudianteIdentificacion, Estudiante estudiante)
        {
            Fecha = fecha;
            Estado = estado;
            EstudianteIdentificacion = estudianteIdentificacion;
            Estudiante = estudiante;
            
        }
        //METODOS
        public void RegistrarAsistencia()
        {
            // Implementación para registrar asistencia
        }

        public void ConsultarAsistencia()
        {
            // Implementación para consultar asistencia
        }
    }

}
