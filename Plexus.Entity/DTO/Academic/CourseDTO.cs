using System;
using Plexus.Database.Enum;
using Plexus.Database.Enum.Academic.Section;

namespace Plexus.Entity.DTO.Academic
{
    public class CreateCourseDTO
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string TranscriptName1 { get; set; }

        public string? TranscriptName2 { get; set; }

        public string? TranscriptName3 { get; set; }

        public Guid AcademicLevelId { get; set; }

        public Guid? FacultyId { get; set; }

        public Guid? DepartmentId { get; set; }
        public Guid? TeachingTypeId { get; set; }
        public Guid? GradeTemplateId { get; set; }

        public decimal Credit { get; set; }
        public decimal RegistrationCredit { get; set; }
        public decimal PaymentCredit { get; set; }

        public decimal Hour { get; set; }
        public decimal LectureCredit { get; set; }
        public decimal LabCredit { get; set; }
        public decimal OtherCredit { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<CourseLocalizationDTO> Localizations { get; set; }
    }

    public class CourseDTO : CreateCourseDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class CourseLocalizationDTO
    {
        public LanguageCode Language { get; set; }

        public string? Name { get; set; }

        public string? TranscriptName1 { get; set; }

        public string? TranscriptName2 { get; set; }

        public string? TranscriptName3 { get; set; }

        public string? Description { get; set; }
    }
}

