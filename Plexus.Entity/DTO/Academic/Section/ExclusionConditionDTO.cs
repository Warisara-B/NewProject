namespace Plexus.Entity.DTO.Academic.Section
{
    public class CreateExclusionConditionDTO
    {
        public Guid SectionId { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<SectionConditionDTO> Conditions { get; set; }
    }

    public class ExclusionConditionDTO : CreateExclusionConditionDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class SectionConditionDTO
    {
        public Guid? FacultyId { get; set; }
        
        public Guid? DepartmentId { get; set; }

        public Guid? CurriculumId { get; set; }

        public Guid? CurriculumVersionId { get; set; }

        public IEnumerable<int>? Batches { get; set; }

        public IEnumerable<string>? Codes { get; set; }
    }
}