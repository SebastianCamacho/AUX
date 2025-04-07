using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class OwnerDTO
    {
        public int Type_Owner { get; set; }
        public ThirdPartyDTO ThirdParty { get; set; } // Aquí llega una instancia de tipo NaturalPersonCreateDTO o LegalEntityCreateDTO

    }

}
