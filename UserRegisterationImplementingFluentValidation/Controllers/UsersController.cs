using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserRegisterationImplementingFluentValidation.Data;
using UserRegisterationImplementingFluentValidation.DTOs;
using UserRegisterationImplementingFluentValidation.Models;
using UserRegisterationImplementingFluentValidation.Validators;

namespace UserRegisterationImplementingFluentValidation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserDbContext _context;

        public UsersController(UserDbContext context)
        {
            _context = context;
        }

        // Registers a new user after validating the provided details.
        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterUser([FromBody] UserDTO createUserDTO)
        {
            // Initialize the validator with the current DbContext.
            var validator = new UserDTOValidator(_context);

            var validationResult = await validator.ValidateAsync(createUserDTO);

            // If validation fails To Return complete error response
            //if (!validationResult.IsValid)
            //{
            //    return BadRequest(validationResult.Errors);
            //}

            // If validation fails, map errors to a simplified response and return a 400 Bad Request.
            if (!validationResult.IsValid)
                {
                    var errorResponse = validationResult.Errors.Select(e => new
                    {
                        Field = e.PropertyName,
                        Error = e.ErrorMessage
                    });

                    return BadRequest(new { Errors = errorResponse });
                }

            // Map the validated DTO to the User entity.
            var user = new User
            {
                FirstName = createUserDTO.FirstName,
                LastName = createUserDTO.LastName,
                Email = createUserDTO.Email,
                Password = createUserDTO.Password,
                DateOfBirth = createUserDTO.DateOfBirth,
                PhoneNumber = createUserDTO.PhoneNumber,
                Address = createUserDTO.Address,
                GenderId = createUserDTO.GenderId,
                CountryId = createUserDTO.CountryId,
                CityId = createUserDTO.CityId,
            };

            // Add the new user to the database.
            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();

            // Return the created user as a response.
            return Ok(user);

        }
    }
}
