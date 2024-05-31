namespace Plexus.Entity.DTO.Payment.Scholarship
{
    public class ScholarshipFeeItemDTO
    {
        public Guid FeeItemId { get; set; }

        public decimal? Percentage { get; set; }

        public decimal? Amount { get; set; }
    }
}