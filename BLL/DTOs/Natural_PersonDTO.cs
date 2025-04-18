using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class Natural_PersonDTO
    {
        public string? thirdParty_Id { get; set; }
        public string? type_Id { get; set; }
        public string? type_Third_Party { get; set; }
        public string? image { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public bool? state { get; set; }
        public DateTime? create_At { get; set; }
        public DateTime? update_At { get; set; }
        public string? create_By { get; set; }
        public string? update_By { get; set; }

        public string? first_Name { get; set; }
        public string? second_Name { get; set; }
        public string? first_Last_Name { get; set; }
        public string? second_Last_Name { get; set; }
    }

    public class Natural_PersonCreateDTO
    {
        public string? thirdParty_Id { get; set; }
        public string? type_Id { get; set; }
        public string? type_Third_Party { get; set; }
        public string? image { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public string? create_By { get; set; }

        public string? first_Name { get; set; }
        public string? second_Name { get; set; }
        public string? first_Last_Name { get; set; }
        public string? second_Last_Name { get; set; }
    }

    public class Natural_PersonUpdateDTO
    {
        public string? thirdParty_Id { get; set; }
        public string? image { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public string? update_By { get; set; }

        public string? first_Name { get; set; }
        public string? second_Name { get; set; }
        public string? first_Last_Name { get; set; }
        public string? second_Last_Name { get; set; }
    }

}
