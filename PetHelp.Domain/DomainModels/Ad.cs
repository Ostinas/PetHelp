namespace PetHelp.Models
{
    public class Ad
    {
        public int? Id { get; set; }

        public string MeetingAddress { get; set; }

        public DateTime CareStart { get; set; }

        public DateTime CareEnd { get; set; }

        public DateTime AdStart { get; set; } = DateTime.Now;

        public DateTime AdEnd { get; set; } = DateTime.Now.AddMonths(1);

        public decimal? Pay { get; set; }

        public int OwnerId { get; set; }

        public Owner? Owner { get; set; }

        public int PetId { get; set; }

        public Pet? Pet { get; set; }

        public List<Applicant>? Applicants { get; set; }

    }
}
