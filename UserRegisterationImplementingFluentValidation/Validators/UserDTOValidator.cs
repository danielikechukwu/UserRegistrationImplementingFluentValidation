using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UserRegisterationImplementingFluentValidation.Data;
using UserRegisterationImplementingFluentValidation.DTOs;

namespace UserRegisterationImplementingFluentValidation.Validators
{
    public class UserDTOValidator : AbstractValidator<UserDTO>
    {
        private readonly UserDbContext _context;

        public UserDTOValidator(UserDbContext context)
        {
            _context = context;

            // ----------------------------
            // Property-Level Validations
            // ----------------------------

            // Validate FirstName: must not be empty and contain only letters.
            RuleFor(user => user.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .Must(name => name.All(char.IsLetter)).WithMessage("First Name must contain only letters.");

            // Validate LastName: must not be empty and contain only letters.
            RuleFor(user => user.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .Must(name => name.All(char.IsLetter)).WithMessage("Last Name must contain only letters.");

            // Validate Email: must not be empty, follow valid email format, and be unique.
            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be in a valid format.")
                .MustAsync(async (email, cancellationToken) =>
                {
                    // Check database to ensure email uniqueness.
                    return !await _context.Users.AnyAsync(u => u.Email == email, cancellationToken);
                })
                .WithMessage("Email must be unique.");

            // Validate Password: must not be empty.
            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Password is required.");

            // Validate PhoneNumber: basic check to ensure it's provided.
            RuleFor(user => user.PhoneNumber)
                .NotEmpty().WithMessage("Phone Number is required.");

            // Validate Address: optional field, but limit the maximum length if provided.
            RuleFor(user => user.Address)
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters.");

            // Validate DateOfBirth: must not be empty, cannot be a future date, and user must be at least 18 years old.
            RuleFor(user => user.DateOfBirth)
                .NotEmpty().WithMessage("Date of Birth is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Date of Birth cannot be a future date.")
                .Must(BeAtLeast18YearsOld)
                .WithMessage("User must be at least 18 years old.");

            // Validate GenderId: asynchronously check that the provided Gender exists.
            RuleFor(user => user.GenderId)
                .MustAsync(IsValidGender).WithMessage("The specified Gender does not exist.");

            // ----------------------------
            // Object-Level Validations
            // ----------------------------

            // Validate that Password and ConfirmPassword match.
            RuleFor(user => user)
                .Custom((dto, context) =>
                {
                    if(dto.Password != dto.ConfirmPassword)
                    {
                        context.AddFailure("Confirm Password must match Password.");
                    }
                });

            // Validate Country and City relationship:
            // 1. Ensure the Country exists.
            // 2. Check that the specified City belongs to that Country.
            RuleFor(user => user)
                .CustomAsync(async (dto, context, cancellationToken) =>
                {
                    // Retrieve the country including its list of cities.
                    var country = await _context.Countries
                        .Include(c => c.Cities)
                        .FirstOrDefaultAsync(c => c.CountryId == dto.CountryId, cancellationToken);

                    if (country == null)
                    {
                        context.AddFailure("CountryId", "The selected country does not exist.");
                    }
                    else if (!country.Cities.Any(city => city.CityId == dto.CityId))
                    {
                        context.AddFailure("CityId", $"The selected city does not belong to the country '{country.Name}'.");
                    }
                });

        }

        // Helper method to check if the user is at least 18 years old.
        private bool BeAtLeast18YearsOld(DateTime dob)
        {
            return dob <= DateTime.Now.AddYears(-18);
        }

        // Asynchronous method to check if the provided GenderId exists in the database.
        private async Task<bool> IsValidGender(int genderId, CancellationToken cancellationToken)
        {
            return await _context.Genders.AnyAsync(g => g.GenderId == genderId, cancellationToken);
        }


    }
}
