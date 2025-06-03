using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class OwnerDTO
    {
        public string type_Owner { get; set; }
        public Third_PartyDTO Third_Party { get; set; }

    }

    public class OwnerCreateDTO
    {
        public string type_Owner { get; set; }
        public Third_PartyCreateDTO Third_Party { get; set; } //Navegacion
        
    }
    public class OwnerUpdateDTO
    {
        public string? type_Owner { get; set; }
        public Third_PartyUpdateDTO Third_Party { get; set; } //Navegacion
        
    }
}
