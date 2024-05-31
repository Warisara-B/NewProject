namespace Plexus.Entity.DTO
{
    public class CreateAudienceGroupDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<AudienceGroupConditionDTO> Conditions { get; set; }

        public bool IsActive { get; set; }
    }

    public class AudienceGroupDTO : CreateAudienceGroupDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class AudienceGroupConditionDTO
    {
        public Guid? AcademicLevelId { get; set; }

        public Guid? AcademicProgramId { get; set; }

        public Guid? FacultyId { get; set; }

        public Guid? DepartmentId { get; set; }

        public IEnumerable<string>? Codes { get; set; }

        public string? StartedCode { get; set; }

        public string? EndedCode { get; set; }

        public int? StartedBatch { get; set; }

        public int? EndedBatch { get; set; }

        public string? StartedLastDigit { get; set; }

        public string? EndedLastDigit { get; set; }
    }
}