using Plexus.Database.Enum.Registration;

namespace Plexus.Entity.DTO.Registration
{
    public class BasePrerequisiteDTO
    {
        public Guid Id { get; set; }

        public IEnumerable<CreatePrerequisiteConditionDTO> Condition { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? DeactivatedAt { get; set; }
    }

    public class CreatePrerequisiteConditionDTO
    {
        public PrerequisiteCondition Condition { get; set; }

        public PrerequisiteConditionType Type { get; set; }

        public Guid? CourseId { get; set; }

        public Guid? GradeId { get; set; }

        public decimal? GPA { get; set; }

        public decimal? Credit { get; set; }

        public int? TermCount { get; set; }

        public Guid? FacultyId { get; set; }

        public Guid? DepartmentId { get; set; }

        public IEnumerable<CreatePrerequisiteConditionDTO>? Conditions { get; set; }
    }
}