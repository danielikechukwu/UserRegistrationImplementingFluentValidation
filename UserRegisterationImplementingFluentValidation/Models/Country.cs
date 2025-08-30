namespace UserRegisterationImplementingFluentValidation.Models
{
    public class Country
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public ICollection<City> Cities { get; set; }
    }
}
