namespace UserRegisterationImplementingFluentValidation.Models
{
    public class City
    {
        public int CityId { get; set; }    // Primary Key
        public string Name { get; set; }   // City name
        public int CountryId { get; set; } // Foreign Key to Country
        // Navigation Property to link with Country entity
        public Country Country { get; set; }
    }
}
