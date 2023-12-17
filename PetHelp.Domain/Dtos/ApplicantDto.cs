namespace PetHelp.Dtos
{
    public class ApplicantDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public int PetId { get; set; }

        public string PetName { get; set; }

        public int AdId { get; set; }
    }
}
