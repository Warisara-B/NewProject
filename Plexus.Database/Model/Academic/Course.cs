using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Model.Academic.Curriculum;
using Plexus.Database.Model.Academic.Faculty;
using Plexus.Database.Model.Localization.Academic;

namespace Plexus.Database.Model.Academic
{
    [Table("Courses")]
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

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

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsActive { get; set; }
        public string? MigrateReference { get; set; }

        [ForeignKey(nameof(AcademicLevelId))]
        public virtual AcademicLevel AcademicLevel { get; set; }

        [ForeignKey(nameof(FacultyId))]
        public virtual Faculty.Faculty? Faculty { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public virtual Department? Department { get; set; }

        [ForeignKey(nameof(TeachingTypeId))]
        public virtual TeachingType? TeachingType { get; set; }

        [ForeignKey(nameof(GradeTemplateId))]
        public virtual GradeTemplate? GradeTemplate { get; set; }

        public virtual IEnumerable<StudyPlanDetail> StudyPlanDetails { get; set; }

        public virtual IEnumerable<CourseLocalization> Localizations { get; set; }
    }
}

