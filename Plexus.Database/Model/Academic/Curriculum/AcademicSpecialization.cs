using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum.Academic.Curriculum;
using Plexus.Database.Model.Localization.Academic.Curriculum;

namespace Plexus.Database.Model.Academic.Curriculum
{
    [Table("AcademicSpecializations")]
	public class AcademicSpecialization
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

        public Guid? ParentAcademicSpecializationId { get; set; }

        public string Name { get; set; }

		public string Code { get; set; }

		public string? Abbreviation { get; set; }

        [Column(TypeName = "nvarchar(100)")]
		public SpecializationType Type { get; set; }

        public string? Description { get; set; }

		public decimal RequiredCredit { get; set; }

		public string? Remark { get; set; }

		public int Level { get; set; }

        public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

		[ForeignKey(nameof(ParentAcademicSpecializationId))]
		public virtual AcademicSpecialization? ParentAcademicSpecialization { get; set; }

        public virtual IEnumerable<SpecializationCourse> SpecializationCourses { get; set; }

		public virtual IEnumerable<AcademicSpecializationLocalization> Localizations { get; set; }
    }
}