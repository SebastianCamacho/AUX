using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY.Models
{
    public class Legal_Entity: Third_Party
    {
        public string? legal_Representative { get; set; }
        public string? name_Company { get; set; }
        public int? check_Digit { get; set; }
        public string? web_Site { get; set; }

        public Legal_Entity()
        {
        }
        
        public Legal_Entity(string id_Third_Party, string type_Id, string image, string type_Third_Party, List<Address> address, string phone, string email, string web_Site, bool state, DateTime create_At, DateTime update_At, string create_By, string update_By, string legal_Representative, string name_Company, int check_Digit)
        {
            this.thirdParty_Id = id_Third_Party;
            this.type_Id = type_Id;
            this.image = image;
            this.type_Third_Party = type_Third_Party;
            Address = address;
            this.phone = phone;
            this.email = email;
            this.web_Site = web_Site;
            this.state = state;
            this.create_At = create_At;
            this.update_At = update_At;
            this.create_By = create_By;
            this.update_By = update_By;
            this.legal_Representative = legal_Representative;
            this.name_Company = name_Company;
            this.check_Digit = check_Digit;
        }

    }
}
