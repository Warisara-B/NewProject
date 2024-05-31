using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Academic.Curriculum
{
    [Table("CurriculumVersionSpecializations")]
    public class CurriculumVersionSpecialization
    {
        public Guid CurriculumVersionId { get; set; }

		public Guid AcademicSpecializationId { get; set; }

        [ForeignKey(nameof(CurriculumVersionId))]
		public virtual CurriculumVersion CurriculumVersion { get; set; }

        [ForeignKey(nameof(AcademicSpecializationId))]
		public virtual AcademicSpecialization AcademicSpecialization { get; set; }
    }
}