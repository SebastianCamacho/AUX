namespace BLL.DTOs
{
    public abstract class ThirdPartyDTO
    {
        public string ThirdParty_Id { get; set; }
        public int Type_Id { get; set; } // 1 = Natural, 2 = Legal
        public string Image { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool State { get; set; }
        public DateTime Create_At { get; set; }
        public DateTime? Update_At { get; set; }
        public string Create_By { get; set; }
        public string? Update_By { get; set; }
    }

    public class NaturalPersonDTO : ThirdPartyDTO
    {
        public string First_Name { get; set; }
        public string? Second_Name { get; set; }
        public string First_Last_Name { get; set; }
        public string? Second_Last_Name { get; set; }
    }

    public class LegalEntityDTO : ThirdPartyDTO
    {
        public string Legal_Representative { get; set; }
        public string Name_Company { get; set; }
        public int Check_Digit { get; set; }
        public string Web_Site { get; set; }
    }
}
