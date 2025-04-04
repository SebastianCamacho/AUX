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
    public abstract class Usuario
    {
        [Key]
        [MaxLength(10)]
        public string Identificacion { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Foto { get; set; }
        [JsonIgnore]
        public User? user { get; set; }

        protected Usuario()
        {
            
        }

        protected Usuario(string identificacion, string? nombres, string? apellidos,  string? foto, int idUser)
        {
            Identificacion = identificacion;
            Nombres = nombres;
            Apellidos = apellidos;
            Foto = foto;
        }

        public void CrearUsuario() { /* implementación */ }
        public void ActualizarUsuario() { /* implementación */ }
        public void EliminarUsuario() { /* implementación */ }
        public void ConsultarUsuario() { /* implementación */ }
    }

}

