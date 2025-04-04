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
    public class User
    {
        [Key]
        public int id { get; set; }
        public string? email { get; set; }
        public string? clave { get; set; }
        public string? salt { get; set; }
        public string? rol { get; set; }
        public string UsuarioIdentificacion { get; set; }
        //Propiedad de navegacion
        public virtual Usuario? Usuario { get; set; }
    

        public User()
        {
            
        }



        public User(int id, string? email, string? clave, string? salt, string? rol, Usuario usuario)
        {
            this.id = id;
            this.email = email;
            this.clave = clave;
            this.salt = salt;
            this.rol = rol;
            Usuario = usuario;
        }
    }
}
