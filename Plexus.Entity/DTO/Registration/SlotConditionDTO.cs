namespace Plexus.Entity.DTO.Registration
{
    public class CreateSlotConditionDTO
    {
        public IEnumerable<ConditionDTO> Conditions { get; set; }

        public bool IsActive { get; set; }
    }

    public class SlotConditionDTO : CreateSlotConditionDTO
    {
        public Guid Id { get; set; }

        public Guid SlotId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class ConditionDTO
    {
        public Guid? FacultyId { get; set; }

        public Guid? DepartmentId { get; set; }

        public IEnumerable<string>? Codes { get; set; }

        public string? StartedCode { get; set; }

        public string? EndedCode { get; set; }

        public int? StartedBatch { get; set; }

        public int? EndedBatch { get; set; }
    }
}