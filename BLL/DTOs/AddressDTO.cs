using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class AddressDTO
    {
        public int Id { get; set; }
        public string Street_Type { get; set; }
        public int Street_Number { get; set; }
        public string Intersection_Number { get; set; }
        public int Property_Number { get; set; }
        public string Neighborhood { get; set; }
        public string Zip_Code { get; set; }
        public string Municipality { get; set; }
        public string City { get; set; }
        public string Third_Id { get; set; }
    }

    public class AddressCreateDTO
    {
        public string Street_Type { get; set; }
        public int Street_Number { get; set; }
        public string Intersection_Number { get; set; }
        public int Property_Number { get; set; }
        public string Neighborhood { get; set; }
        public string Zip_Code { get; set; }
        public string Municipality { get; set; }
        public string City { get; set; }
        public string Third_Id { get; set; }
    }

    public class AddressUpdateDTO
    {
        public string? Street_Type { get; set; }
        public int? Street_Number { get; set; }
        public string? Intersection_Number { get; set; }
        public int? Property_Number { get; set; }
        public string? Neighborhood { get; set; }
        public string? Zip_Code { get; set; }
        public string? Municipality { get; set; }
        public string? City { get; set; }
        public string? Third_Id { get; set; }
    }
}
