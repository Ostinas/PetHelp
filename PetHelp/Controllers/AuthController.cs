using Microsoft.AspNetCore.Mvc;
using PetHelp.Dtos;
using PetHelp.Repositories;

namespace PetHelp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly AuthRepository _authRepository;
        private readonly AuthHelper authHelper;

        public AuthController(ILogger<AuthController> logger, AuthRepository authRepository, IConfiguration config)
        {
            _logger = logger;
            _authRepository = authRepository;
            authHelper = new AuthHelper(config);
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> RegisterApplicant([FromBody] OwnerDto registrationData)
        //{
        //    var hashedPasswordAndSalt = AuthHelper.HashPassword(registrationData.Password);

        //    var owner = new Owner
        //    {
        //        Name = registrationData.Name,
        //        Email = registrationData.Email,
        //        //Salt = hashedPasswordAndSalt.salt,
        //        Password = hashedPasswordAndSalt.hashedPassword,
        //        PhoneNumber = registrationData.PhoneNumber,
        //        Address = registrationData.Address,
        //        City = registrationData.City,
        //    };

        //    try
        //    {
        //        _authRepository.AddUser(owner);

        //        var token = authHelper.GenerateJwtToken(owner.Id, "Candidate");
        //        return Created(string.Empty, new { Token = token });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginData)
        {
            var role = loginData.Role.ToLower();
            var userId = 0;
            var password = string.Empty;

            if (role == "applicant")
            {
                var applicant = await _authRepository.GetApplicant(loginData.Email);

                if (applicant == null)
                {
                    return NotFound("Account not found.");
                }

                userId = (int)applicant.Id;
                password = applicant.Password;
            }
            else if (role == "owner")
            {
                var owner = await _authRepository.GetOwner(loginData.Email);

                if (owner == null)
                {
                    return NotFound("Account not found.");
                }

                userId = (int)owner.Id;
                password = owner.Password;
            }
            else if (role == "admin")
            {
                var token = authHelper.GenerateJwtToken(-1, "admin");
                return Created(string.Empty, new { Token = token });
            }

            if (authHelper.DoesPasswordMatch(loginData.Password, password))
            {
                var token = authHelper.GenerateJwtToken(userId, role);
                return Created(string.Empty, new { Token = token });
            }
            return Unauthorized();
        }
    }
}
