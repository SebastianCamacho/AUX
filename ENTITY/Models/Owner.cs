using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY.Models
{
   public class Owner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? id_ { get; set; }
        public string? type_Owner { get; set; } //'Propietario y tenedor' 'propietario' 'tenedor'

        [Required]
        public string third_Id { get; set; }
        [ForeignKey("third_Id")]
        public Third_Party? Third_Party { get; set; } //Navegacion 

        public Owner()
        {
            
        }

        public Owner(string type_Owner, Third_Party third_)
        {
            this.type_Owner = type_Owner;
           this.Third_Party = third_;
        }
    }
}
