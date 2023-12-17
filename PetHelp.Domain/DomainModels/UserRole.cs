using PetHelp.Domain.Enums;

namespace PetHelp.Domain.DomainModels
{
    public class UserRole
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public Role Role { get; set; }
    }
}
