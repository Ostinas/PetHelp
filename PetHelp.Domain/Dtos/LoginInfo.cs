using System.ComponentModel.DataAnnotations;

namespace PetHelp.Domain
{
    public class LoginInfo
    {
        [Required]
        [EmailAddress]
        public string Email {  get; set; }

        [Required]
        public string Password { get; set; }
    }
}
