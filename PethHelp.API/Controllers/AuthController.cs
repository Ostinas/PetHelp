using Microsoft.AspNetCore.Mvc;
using PetHelp.Domain;
using PetHelp.API.Repositories;

namespace PetHelp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(AuthRepository authRepository, IConfiguration config) : ControllerBase
    {
        private readonly AuthRepository _authRepository = authRepository;
        private readonly AuthHelper authHelper = new(config);

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginInfo loginData)
        {
            var applicant = await _authRepository.GetApplicant(loginData.Email);

            if (applicant != null)
            {
                if (authHelper.DoesPasswordMatch(loginData.Password, applicant.Password))
                {
                    return Created(string.Empty, new { Token = authHelper.GenerateJwtToken((int)applicant.Id, "applicant") });
                }
            }

            var owner = await _authRepository.GetOwner(loginData.Email);

            if (owner != null)
            {
                if (authHelper.DoesPasswordMatch(loginData.Password, owner.Password))
                {
                    return Created(string.Empty, new { Token = authHelper.GenerateJwtToken((int)owner.Id, "owner") });
                }
            }

            var admin = await _authRepository.GetAdmin(loginData.Email);

            if (admin != null && authHelper.DoesPasswordMatch(loginData.Password, admin.Password))
            {
                var token = authHelper.GenerateJwtToken(-1, "admin");
                return Created(string.Empty, new { Token = token });
            }

            if (applicant == null && owner != null && admin == null)
            {
                return NotFound("Account not found.");
            }

            return Unauthorized();
        }
    }
}
