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
    public class Grupo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? idGrupo { get; set; }
        public string? Descripcion { get; set; }

        public string? ProfesorIdentificacion { get; set; }
        //PROPIEDAD NAVEGACIONAL
        public Profesor? profesor { get; set; }

        public string AsignaturaCodigo { get; set; }
        //PROPIEDAD NAVEGACIONAL
        public Asignatura? asignatura { get; set; }


        public List<Matricula> Matriculas { get; set; }
        public List<Clase>? Clases { get; set; }
        public List<Horario>? Horarios { get; set; }




        public Grupo()
        {
            
        }

        public Grupo(Asignatura asignatura)
        {
            this.asignatura = asignatura;
        }

        public Grupo(int idGrupo)
        {
            this.idGrupo = idGrupo;
        }
    }
}
