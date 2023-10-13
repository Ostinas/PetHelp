using PetHelp.Models;

namespace PetHelp.Dtos
{
    public class PetDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Sex { get; set; }

        public int Age { get; set; }

        public string OwnerName { get; set; }

        public DateTime? CareStart { get; set; }

        public DateTime? CareEnd { get; set; }
    }
}
