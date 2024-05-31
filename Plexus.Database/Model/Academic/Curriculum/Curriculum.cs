using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Model.Academic.Faculty;
using Plexus.Database.Model.Localization.Academic.Curriculum;

namespace Plexus.Database.Model.Academic.Curriculum
{
	[Table("Curriculums")]
	public class Curriculum
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public Guid AcademicLevelId { get; set; }

		public Guid FacultyId { get; set; }

		public Guid? DepartmentId { get; set; }

		public string Name { get; set; }

		public string Code { get; set; }

		public string? FormalName { get; set; }

		public string? Abbreviation { get; set; }

		public string? Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		public DateTime UpdatedAt { get; set; }

		public string UpdatedBy { get; set; }

        [ForeignKey(nameof(AcademicLevelId))]
        public virtual AcademicLevel AcademicLevel { get; set; }

        [ForeignKey(nameof(FacultyId))]
        public virtual Faculty.Faculty Faculty { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public virtual Department? Department { get; set; }

        public virtual IEnumerable<CurriculumVersion> Versions { get; set; }

		public virtual IEnumerable<CurriculumLocalization> Localizations { get; set; }
	}
}

