using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public abstract class Third_Party
    {
        [Key]
        [Required]
        public string? thirdParty_Id { get; set; }
        public string? type_Id { get; set; }
        public string? type_Third_Party { get; set; }
        public string? image { get; set; }
        public ICollection<Address>? Address { get; set; } = new List<Address>(); // Navegacion
        public string? phone { get; set; }
        public string? email { get; set; }
        public bool? state { get; set; }
        public DateTime? create_At { get; set; }
        public DateTime? update_At { get; set; }
        public string? create_By { get; set; }
        public string? update_By { get; set; }

        public Third_Party()
        {

        }

       
        public Third_Party(string id, string type_Id, string type_Third_Party, string image, ICollection<Address> address, string phone, string email, bool state, DateTime create_At, DateTime update_At, string create_By, string update_By)
        {
            this.thirdParty_Id = id;
            this.type_Id = type_Id;
            this.type_Third_Party = type_Third_Party;
            this.image = image;
            Address = address;
            this.phone = phone;
            this.email = email;
            this.state = state;
            this.create_At = create_At;
            this.update_At = update_At;
            this.create_By = create_By;
            this.update_By = update_By;
        }
    }
}
