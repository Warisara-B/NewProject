using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum.Academic.Section;
using Plexus.Database.Model.Academic.Curriculum;
using Plexus.Database.Model.Academic.Faculty;
using Plexus.Database.Model.Facility;

namespace Plexus.Database.Model.Academic.Section
{
	[Table("Sections")]
	public class Section
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public Guid AcademicLevelId { get; set; }

		public Guid TermId { get; set; }

		public Guid CourseId { get; set; }

		public Guid CampusId { get; set; }

		public Guid? ParentSectionId { get; set; }

		public Guid? FacultyId { get; set; }

		public Guid? DepartmentId { get; set; }

		public Guid? CurriculumVersionId { get; set; }

		public string SectionNo { get; set; }

		public int SeatLimit { get; set; }

		public int? AvailableSeat { get; set; }

		public bool IsWithdrawable { get; set; }

		public bool IsInvisibled { get; set; }

		public bool IsClosed { get; set; }

		public string? Remark { get; set; }

		public string? Batch { get; set; }

		public string? StudentCodes { get; set; }

		public DateTime? StartedAt { get; set; }

		public DateTime? EndedAt { get; set; }

		[Column(TypeName = "nvarchar(100)")]
		public SectionStatus Status { get; set; }

		[Column(TypeName = "nvarchar(100)")]
		public SectionType Type { get; set; }

		public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		public DateTime UpdatedAt { get; set; }

		public string UpdatedBy { get; set; }

		[ForeignKey(nameof(AcademicLevelId))]
		public AcademicLevel AcademicLevel { get; set; }

		[ForeignKey(nameof(TermId))]
		public virtual Term Term { get; set; }

		[ForeignKey(nameof(CourseId))]
		public virtual Course Course { get; set; }

		[ForeignKey(nameof(CampusId))]
		public virtual Campus Campus { get; set; }

		[ForeignKey(nameof(ParentSectionId))]
		public virtual Section? ParentSection { get; set; }

		[ForeignKey(nameof(FacultyId))]
		public virtual Faculty.Faculty? Faculty { get; set; }

		[ForeignKey(nameof(DepartmentId))]
		public virtual Department? Department { get; set; }

		[ForeignKey(nameof(CurriculumVersionId))]
		public virtual CurriculumVersion? CurriculumVersion { get; set; }

		public virtual IEnumerable<SectionInstructor> Instructors { get; set; }

		public virtual IEnumerable<Section> JointSections { get; set; }

		public virtual IEnumerable<SectionClassPeriod> SectionClassPeriods { get; set; }

		public virtual IEnumerable<SectionExamination> SectionExaminations { get; set; }
	}
}

