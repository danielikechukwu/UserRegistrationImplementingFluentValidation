namespace UserRegisterationImplementingFluentValidation.Models
{
    public class Country
    {
        public int CountryId { get; set; } // Primary Key
        public string Name { get; set; }   // Country name
        public ICollection<City> Cities { get; set; } // List of cities in this country
    }
}
