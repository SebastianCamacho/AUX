using ENTITY.Models_Global;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY.Models
{
    public class Driver
    {
        [Key] 
        public string driver_Id { get; set; }
        [Required]
        public string type_Id { get; set; }         // Ejemplo: "CC", "CE"
        public string? image { get; set; }           // Nullable
        [Required]
        public string first_Name { get; set; }
        [Required]
        public string first_Last_Name { get; set; }
        public string? second_Name { get; set; }     // Nullable
        public string? second_Last_Name { get; set; }    // Nullable
        public string? phone { get; set; }           // Nullable
        public string? email { get; set; }           // Nullable
        public string? web_Site { get; set; }        // Nullable

        public bool state { get; set; }
        public DateTime create_At { get; set; }
        public DateTime update_At { get; set; }
        public string create_By { get; set; }
        public string update_By { get; set; }

        // Relación: Un Driver puede tener muchos Document_Driver
        public virtual ICollection<Document_Driver> Document_Drivers { get; set; } 

        public Driver()
        {
            Document_Drivers = new List<Document_Driver>();
        }

        public Driver(string driver_Id, string type_Id, string? image, string first_Name, string first_Last_Name, string? second_Name, string? second_Last_Name, string? phone, string? email, string? web_Site, bool state, DateTime create_At, DateTime update_At, string create_By, string update_By, ICollection<Document_Driver> document_Drivers)
        {
            this.driver_Id = driver_Id;
            this.type_Id = type_Id;
            this.image = image;
            this.first_Name = first_Name;
            this.first_Last_Name = first_Last_Name;
            this.second_Name = second_Name;
            this.second_Last_Name = second_Last_Name;
            this.phone = phone;
            this.email = email;
            this.web_Site = web_Site;
            this.state = state;
            this.create_At = create_At;
            this.update_At = update_At;
            this.create_By = create_By;
            this.update_By = update_By;
            Document_Drivers = document_Drivers;
        }
    }
}
