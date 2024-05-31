using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Enum.Student;
using Plexus.Database.Model.Academic.Faculty;

namespace Plexus.Database.Model.Registration
{
	[Table("Conditions")]
	public class Condition
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		// JSON string array
		public string AllowStudentCodes { get; set; }

		public Guid? AcademicLevelId { get; set; }

		public Guid? FacultyId { get; set; }

		public Guid? DepartmentId { get; set; }

		public string FromCode { get; set; }

		public string ToCode { get; set; }

		public int? FromBatch { get; set; }

		public int? ToBatch { get; set; }

		public AdmissionType? AdmissionType { get; set; }

		public AcademicStatus? AcademicStatus { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }

		[ForeignKey(nameof(FacultyId))]
		public virtual Faculty Faculty { get; set; }

		[ForeignKey(nameof(DepartmentId))]
		public virtual Department Department { get; set; }
	}
}

