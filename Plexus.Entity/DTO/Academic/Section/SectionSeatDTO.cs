using Plexus.Database.Enum.Academic.Section;

namespace Plexus.Entity.DTO.Academic.Section
{
    public class CreateSectionSeatDTO
    {
        public string Name { get; set; }

        public SeatType Type { get; set; }

        public IEnumerable<SectionConditionDTO>? Conditions { get; set; }

        public int TotalSeat { get; set; }

        public string? Remark { get; set; }
    }
    
    public class SectionSeatDTO : UpsertSectionSeatDTO
    {
        public new Guid Id { get; set; }

        public Guid SectionId { get; set; }

        public int SeatUsed { get; set; }

        public Guid? MasterSeatId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int? UsageAmount { get; set; }
    }
    
    public class UpsertSectionSeatDTO
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public SeatType Type { get; set; }

        public IEnumerable<SectionConditionDTO>? Conditions { get; set; }

        public int TotalSeat { get; set; }

        public string? Remark { get; set; }
    }
}

