using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class Contracting_PartyDTO
    {
        public string? signature_Image { get; set; }
        public Third_PartyDTO Third_Party { get; set; }

    }

    public class Contracting_PartyCreateDTO
    {
        public string? signature_Image { get; set; }
        public Third_PartyCreateDTO Third_Party { get; set; } //Navegacion
    }

    public class Contracting_PartyUpdateDTO
    {
        public string? signature_Image { get; set; }
        public Third_PartyUpdateDTO Third_Party { get; set; } //Navegacion
    }

    
}
