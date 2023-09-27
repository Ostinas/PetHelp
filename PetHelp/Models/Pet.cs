using System.Drawing;

namespace PetHelp.Models
{
    public class Pet
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Sex { get; set; }

        public int Age { get; set; }

        public int UserId { get; set; } 

        public User User { get; set; }

        public int? AdId { get; set; }

        public Ad Ad { get; set; }
    }
}
