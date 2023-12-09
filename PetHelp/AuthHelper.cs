using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
#pragma warning disable CS8601 // Possible null reference assignment.
namespace PetHelp
{
    public class AuthHelper
    {
        private readonly IConfiguration _configuration;
        private readonly string jwtKey;
        private readonly string jwtIssuer;

        public AuthHelper(IConfiguration configuration)
        {
            _configuration = configuration;

            jwtKey = _configuration.GetValue<string>("Jwt_Key");
            jwtIssuer = _configuration.GetValue<string>("Jwt_Issuer");
        }

        //public static (string salt, string hashedPassword) HashPassword(string password)
        //{
        //    byte[] passwordPlain = Encoding.UTF8.GetBytes(password);
        //    byte[] passwordSalt = RandomNumberGenerator.GetBytes(32);

        //    string saltString, passwordHashString;
        //    using (var pbkdf2 = new Rfc2898DeriveBytes(passwordPlain, passwordSalt, 10000, HashAlgorithmName.SHA512))
        //    {
        //        byte[] hash = pbkdf2.GetBytes(32);
        //        saltString = Convert.ToBase64String(passwordSalt);
        //        passwordHashString = Convert.ToBase64String(hash);
        //    }
        //    return (saltString, passwordHashString);
        //}

        public bool DoesPasswordMatch(string loginPassword, string storedPassword)
        {
            return loginPassword == storedPassword;
        }

        public string GenerateJwtToken(int userId, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = jwtIssuer,
                Audience = "PetHelp",
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static int GetUserId(ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || int.TryParse(userIdClaim.Value, out int userId) == false)
            {
                return 0;
            }
            return userId;
        }
    }
}
