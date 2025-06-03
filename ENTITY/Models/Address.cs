using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENTITY
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string? street_Type { get; set; }
        public int? street_Number { get; set; }
        public string? intersection_Number { get; set; }
        public int? property_Number { get; set; }
        public string? neighborhood { get; set; }
        public string? zip_Code { get; set; }
        public string? municipality { get; set; }
        public string? city { get; set; }

        [Required]
        public string third_Id { get; set; }
        [ForeignKey("third_Id")]
        public Third_Party? Third_Party { get; set; } //Navegacion inversa
        

        public Address(string third_Id)
        {
            this.third_Id = third_Id;

        }




        public Address(int id, string street_Type, int street_Number, string intersection_Number, int property_Number, string neighborhood, string zip_Code, string municipality, string city, string third_Id)
        {
            this.id = id;
            this.street_Type = street_Type;
            this.street_Number = street_Number;
            this.intersection_Number = intersection_Number;
            this.property_Number = property_Number;
            this.neighborhood = neighborhood;
            this.zip_Code = zip_Code;
            this.municipality = municipality;
            this.city = city;
            this.third_Id = third_Id;

        }
    }
}