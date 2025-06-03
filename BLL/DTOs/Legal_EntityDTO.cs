using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class Legal_EntityDTO : Third_PartyDTO
    {
        

        // Propiedades propias de Legal_Entity
        public string? legal_Representative { get; set; }
        public string? name_Company { get; set; }
        public int? check_Digit { get; set; }
        public string? web_Site { get; set; }
    }


    public class Legal_EntityCreateDTO :Third_PartyCreateDTO
    {
        public Legal_EntityCreateDTO()
        {
            type_Third_Party = "Persona Juridica";
        }
        public string? legal_Representative { get; set; }
        public string? name_Company { get; set; }
        public int? check_Digit { get; set; }
        public string? web_Site { get; set; }
    }

    public class Legal_EntityUpdateDTO : Third_PartyUpdateDTO
    {
        public Legal_EntityUpdateDTO()
        {
            type_Third_Party = "Persona Juridica";
        }

        public string? legal_Representative { get; set; }
        public string? name_Company { get; set; }
        public int? check_Digit { get; set; }
        public string? web_Site { get; set; }
    }

}
