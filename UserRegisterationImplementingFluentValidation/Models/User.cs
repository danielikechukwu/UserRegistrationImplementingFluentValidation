namespace UserRegisterationImplementingFluentValidation.Models
{
    public class User
    {
        public int UserId { get; set; }          // Primary Key
        public string FirstName { get; set; }      // User's first name
        public string LastName { get; set; }       // User's last name
        public string Email { get; set; }          // User's email address and must be Unique
        public string Password { get; set; }       // User's password (should be hashed in production)
        public DateTime DateOfBirth { get; set; }    // User's birth date
        public string PhoneNumber { get; set; }    // Contact phone number
        public string Address { get; set; }        // Optional: User’s address
        public int? GenderId { get; set; }         // Foreign Key to Gender
        public int? CountryId { get; set; }        // Foreign Key to Country
        public int? CityId { get; set; }           // Foreign Key to City (must belong to selected Country)
        // Navigation Properties
        public Gender Gender { get; set; }
        public Country Country { get; set; }
        public City City { get; set; }
    }
}
