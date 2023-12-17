namespace PetHelp.Dtos
{
    public class AdDto
    {
        public string MeetingAddress { get; set; }

        public DateTime CareStart { get; set; }

        public DateTime CareEnd { get; set; }

        public decimal? Pay { get; set; }

        public string OwnerName { get; set; }

        public string PetName { get; set; }

        public int PetId { get; set; }

        public int Id { get; set; }
    }
}
