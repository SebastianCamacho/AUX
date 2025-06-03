using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class Natural_PersonDTO : Third_PartyDTO
    {
       
        public string? first_Name { get; set; }
        public string? second_Name { get; set; }
        public string? first_Last_Name { get; set; }
        public string? second_Last_Name { get; set; }
    }

    public class Natural_PersonCreateDTO : Third_PartyCreateDTO
    {
        public Natural_PersonCreateDTO()
        {
            type_Third_Party = "Persona Natural";
        }
        public string? first_Name { get; set; }
        public string? second_Name { get; set; }
        public string? first_Last_Name { get; set; }
        public string? second_Last_Name { get; set; }
    }

    public class Natural_PersonUpdateDTO : Third_PartyUpdateDTO
    {
        public Natural_PersonUpdateDTO()
        {
            type_Third_Party = "Persona Natural";
        }
        public string? first_Name { get; set; }
        public string? second_Name { get; set; }
        public string? first_Last_Name { get; set; }
        public string? second_Last_Name { get; set; }
    }

}
