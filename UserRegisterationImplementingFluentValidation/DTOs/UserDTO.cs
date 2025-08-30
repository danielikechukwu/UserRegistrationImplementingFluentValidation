namespace UserRegisterationImplementingFluentValidation.DTOs
{
    public class UserDTO
    {
        // Personal details
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Contact and security details
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        // Demographics
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public int GenderId { get; set; }

        // Address details (optional)
        public string Address { get; set; }

        // Location details
        public int CountryId { get; set; }
        public int CityId { get; set; } // CityId must belong to the specified Country
    }
}
