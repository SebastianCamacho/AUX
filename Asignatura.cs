using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ENTITY
{
    public class Asignatura
    {
        [Key]
        public string Codigo { get; set; }
        public string NombreAsignatura { get; set; }
        [JsonIgnore]
        public List<Grupo>? Grupos { get; set; }

        public Asignatura()
        {

        }
    
        public Asignatura(string nombreAsignatura, string codigo)
        {
            NombreAsignatura = nombreAsignatura ?? throw new ArgumentNullException(nameof(nombreAsignatura));
            Codigo = codigo ?? throw new ArgumentNullException(nameof(codigo));
        }

        public void CrearAsignatura()
        {
            // Implementación para crear asignatura
        }

        public void ActualizarAsignatura()
        {
            // Implementación para actualizar asignatura
        }

        public void EliminarAsignatura()
        {
            // Implementación para eliminar asignatura
        }

        public void ConsultarAsignatura()
        {
            // Implementación para consultar asignatura
        }
    }

}
