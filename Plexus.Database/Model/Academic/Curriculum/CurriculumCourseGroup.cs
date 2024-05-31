using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum.Academic.Curriculum;
using Plexus.Database.Model.Localization.Academic.Curriculum;

namespace Plexus.Database.Model.Academic.Curriculum
{
	[Table("CurriculumCourseGroups")]
	public class CurriculumCourseGroup
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public Guid CurriculumVersionId { get; set; }

		public Guid? ParentCourseGroupId { get; set; }

		public string Name { get; set; }

		[Column(TypeName = "nvarchar(100)")]
		public CourseGroupType Type { get; set; }

		public string? Description { get; set; }

		public decimal RequiredCredit { get; set; }

		public string? Remark { get; set; }

		public int Sequence { get; set; }

		[Column(TypeName = "nvarchar(500)")]
		public string? MigrationReference { get; set; }

		public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		public DateTime UpdatedAt { get; set; }

		public string UpdatedBy { get; set; }

		[ForeignKey(nameof(CurriculumVersionId))]
		public virtual CurriculumVersion CurriculumVersion { get; set; }

		[ForeignKey(nameof(ParentCourseGroupId))]
		public virtual CurriculumCourseGroup? ParentCourseGroup { get; set; }

		public virtual IEnumerable<CurriculumCourse> CurriculumCourses { get; set; }

		public virtual IEnumerable<CurriculumCourseGroupIgnoreCourse> CourseGroupIgnoreCourses { get; set; }

		public virtual IEnumerable<CurriculumCourseGroupLocalization> Localizations { get; set; }

	}
}

