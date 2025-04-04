using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class Contracting_Party
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? id_ { get; set; }
        public string? signature_Image { get; set; }


        [Required]
        public string third_Id { get; set; }
        [ForeignKey("third_Id")]
        public Third_Party? Third_Party { get; set; } //Navegacion 

        public Contracting_Party()
        {
        }

        public Contracting_Party(string signature_Image, Third_Party third_)
        {
            this.signature_Image = signature_Image;
            this.Third_Party = third_;
        }

    }
}
