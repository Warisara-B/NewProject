using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Academic.Curriculum
{
    [Table("SpecializationCourses")]
    public class SpecializationCourse
    {
        public Guid AcademicSpecializationId { get; set; }

		public Guid CourseId { get; set; }

		public Guid? RequiredGradeId { get; set; }

		public bool IsRequiredCourse { get; set; }

		[ForeignKey(nameof(AcademicSpecializationId))]
		public virtual AcademicSpecialization AcademicSpecialization { get; set; }

		[ForeignKey(nameof(CourseId))]
		public virtual Course Course { get; set; }

		[ForeignKey(nameof(RequiredGradeId))]
		public virtual Grade? RequiredGrade { get; set; }
    }
}