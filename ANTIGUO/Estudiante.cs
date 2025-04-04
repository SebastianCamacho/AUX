using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class Estudiante : Usuario
    {
        public string Carrera { get; set; }
        public List<Matricula> Matriculas { get; set; }
        public List<Asistencia> Asistencias { get; set; }


        public Estudiante()
        {
            
        }




        public Estudiante(string identificacion, string nombres, string apellidos, string? foto, int idUser, string carrera)
        {
            Identificacion = identificacion ?? throw new ArgumentNullException(nameof(identificacion));
            Nombres = nombres ?? throw new ArgumentNullException(nameof(nombres));
            Apellidos = apellidos ?? throw new ArgumentNullException(nameof(apellidos));
            Foto = foto; // Foto es opcional, no necesita verificación
            Carrera = carrera ?? throw new ArgumentNullException(nameof(carrera));
        }

        public void CrearUsuario()
        {
            // Implementación para crear un estudiante
        }

        public void ActualizarUsuario()
        {
            // Implementación para actualizar un estudiante
        }

        public void EliminarUsuario()
        {
            // Implementación para eliminar un estudiante
        }

        public void ConsultarUsuario()
        {
            // Implementación para consultar un estudiante
        }
    }
}
