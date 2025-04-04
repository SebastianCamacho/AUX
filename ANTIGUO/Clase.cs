using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ENTITY
{
    public class Clase
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idClase { get; set; }
        public DateTime fecha { get; set; }


        public int GrupoidGrupo { get; set; }
        public Grupo grupo { get; set; }


        public List<Asistencia> Asistencias { get; set; }


        //CONSTRUCTORES
        public Clase(){
        }

        public Clase(int idClase, DateTime fecha, int grupoidGrupo, Grupo grupo, List<Asistencia> asistencias) : this(idClase, fecha)
        {
            GrupoidGrupo = grupoidGrupo;
            this.grupo = grupo;
            Asistencias = asistencias;
        }

        public Clase(int grupoidGrupo, Grupo grupo)
        {
            GrupoidGrupo = grupoidGrupo;
            this.grupo = grupo;
        }

        public Clase(int idClase, DateTime fecha)
        {
            this.idClase = idClase;
            this.fecha = fecha;
        }

        //METODOS
        public void CrearClase()
        {
        }

        public void ActualizarClase()
        {
        }

        public void EliminarClase()
        {
        }

        public void ConsultarClase()
        {
        }
    }

}
