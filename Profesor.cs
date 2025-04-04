using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ENTITY
{
    public class Profesor : Usuario
    {
        public string Profesion { get; set; }
        public List<Grupo>? Grupos { get; set; }  // Puede ser null

        public Profesor()
        {

        }

        public Profesor(string identificacion, string nombres, string apellidos, string? foto, string profesion, List<Grupo>? grupos = null)
        {
            Identificacion = identificacion ?? throw new ArgumentNullException(nameof(identificacion));
            Nombres = nombres ?? throw new ArgumentNullException(nameof(nombres));
            Apellidos = apellidos ?? throw new ArgumentNullException(nameof(apellidos));
            Foto = foto;
            Profesion = profesion ?? throw new ArgumentNullException(nameof(profesion));
            Grupos = grupos;  
        }




        public void CrearUsuario()
        {
            // Implementación para crear un profesor
        }

        public void ActualizarUsuario()
        {
            // Implementación para actualizar un profesor
        }

        public void EliminarUsuario()
        {
            // Implementación para eliminar un profesor
        }

        public void ConsultarUsuario()
        {
            // Implementación para consultar un profesor
        }
    }
}
