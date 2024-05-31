namespace Plexus.Entity.DTO.Payment.Scholarship
{
    public class StudentScholarshipUsageDTO
    {
        public long Id { get; set; }
        
        public Guid ScholarshipId { get; set; }

        public int? Year { get; set; }

        public int? Term { get; set; }

        public string? DocumentNumber { get; set; }

        public decimal Amount { get; set; }

        public string? Remark { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}