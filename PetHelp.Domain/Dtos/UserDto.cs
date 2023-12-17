using PetHelp.Domain.DomainModels;

namespace PetHelp.Domain.Dtos
{
    public class UserDto
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public UserRole Role { get; set; }
    }
}
