using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum.Academic;
using Plexus.Database.Model.Academic.Faculty;
using Plexus.Database.Model.Localization.Academic.Curriculum;
using Plexus.Database.Model.Registration;

namespace Plexus.Database.Model.Academic.Curriculum
{
	[Table("CurriculumVersions")]
	public class CurriculumVersion
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public Guid? CurriculumId { get; set; }
		public Guid AcademicLevelId { get; set; }
		public Guid FacultyId { get; set; }
		public Guid DepartmentId { get; set; }
		public Guid AcademicProgramId { get; set; }

		public string Code { get; set; }

		public string Name { get; set; }

		public string? DegreeName { get; set; }

		public string? Description { get; set; }

		public string? Abbreviation { get; set; }
		public decimal TotalYear { get; set; }
		public decimal TotalCredit { get; set; }
		public decimal ExpectedGraduatingCredit { get; set; }
		public DateTime ApprovedAt { get; set; }
		public int StartBatchCode { get; set; }
		public int EndBatchCode { get; set; }

		[Column(TypeName = "nvarchar(100)")]
		public CollegeCalendarType CollegeCalendarType { get; set; } = CollegeCalendarType.SEMESTER;

		public string? Remark { get; set; }

		public bool IsActive { get; set; }

		public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		public DateTime UpdatedAt { get; set; }

		public string UpdatedBy { get; set; }

		[ForeignKey(nameof(CurriculumId))]
		public virtual Curriculum? Curriculum { get; set; }

		[ForeignKey(nameof(AcademicLevelId))]
		public virtual AcademicLevel AcademicLevel { get; set; }

		[ForeignKey(nameof(FacultyId))]
		public virtual Faculty.Faculty Faculty { get; set; }

		[ForeignKey(nameof(DepartmentId))]
		public virtual Department Department { get; set; }

		[ForeignKey(nameof(AcademicProgramId))]
		public virtual AcademicProgram AcademicProgram { get; set; }

		public virtual IEnumerable<CurriculumCourseGroup> CourseGroups { get; set; }

		public virtual IEnumerable<StudyPlan> StudyPlans { get; set; }

		public virtual IEnumerable<CurriculumCoursePrerequisite> CoursePrerequisites { get; set; }

		public virtual IEnumerable<CurriculumInstructor> CurriculumInstructors { get; set; }

		public virtual IEnumerable<CurriculumVersionLocalization> Localizations { get; set; }
	}
}

