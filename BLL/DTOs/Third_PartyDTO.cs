using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type_Third_Party")]
    [JsonDerivedType(typeof(Natural_PersonDTO), typeDiscriminator: "Persona Natural")]
    [JsonDerivedType(typeof(Legal_EntityDTO), typeDiscriminator: "Persona Jurídica")]
    public class Third_PartyDTO
    {
        public string? thirdParty_Id { get; set; }
        public string? type_Id { get; set; }
        public string? type_Third_Party { get; set; } // Discriminador
        public string? image { get; set; }
        public List<AddressDTO>? Address { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public bool? state { get; set; }
        public DateTime? create_At { get; set; }
        public DateTime? update_At { get; set; }
        public string? create_By { get; set; }
        public string? update_By { get; set; }

       
    }



    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type_Third_Party")]
    [JsonDerivedType(typeof(Natural_PersonCreateDTO), typeDiscriminator: "Persona Natural")]
    [JsonDerivedType(typeof(Legal_EntityCreateDTO), typeDiscriminator: "Persona Juridica")]
    public abstract class Third_PartyCreateDTO
    {
        // Propiedades de Third_Party
        public string? type_Third_Party { get; set; } // Discriminador
        public string? thirdParty_Id { get; set; }
        public string? type_Id { get; set; }
        public string? image { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public string? create_By { get; set; }
    }


    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type_Third_Party")]
    [JsonDerivedType(typeof(Natural_PersonUpdateDTO), typeDiscriminator: "Persona Natural")]
    [JsonDerivedType(typeof(Legal_EntityUpdateDTO), typeDiscriminator: "Persona Juridica")]
    public abstract class Third_PartyUpdateDTO
    {
        public string? type_Third_Party { get; set; } // Discriminador
        public string? thirdParty_Id { get; set; }
        public string? type_Id { get; set; }
        public string? image { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public bool? state { get; set; }
    }

}




