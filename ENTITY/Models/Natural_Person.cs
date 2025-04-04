using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY.Models
{
    public class Natural_Person : Third_Party
    {
        public string? first_Name { get; set; }
        public string? second_Name { get; set; }
        public string? first_Last_Name { get; set; }
        public string? second_Last_Name { get; set; }

        public Natural_Person() { }
        
        public Natural_Person(string id_Third_Party, string type_Id, string image, string type_Third_Party, List<Address> address, string phone, string email, bool state, DateTime create_At, DateTime update_At, string create_By, string update_By, string first_Name, string second_Name, string first_Last_Name, string second_Last_Name)
        {
            this.thirdParty_Id = id_Third_Party;
            this.type_Id = type_Id;
            this.image = image;
            this.type_Third_Party = type_Third_Party;
            Address = address;
            this.phone = phone;
            this.email = email;
            this.state = state;
            this.create_At = create_At;
            this.update_At = update_At;
            this.create_By = create_By;
            this.update_By = update_By;
            this.first_Name = first_Name;
            this.second_Name = second_Name;
            this.first_Last_Name = first_Last_Name;
            this.second_Last_Name = second_Last_Name;
        }
    }

}
