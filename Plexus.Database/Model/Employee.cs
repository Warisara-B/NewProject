using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Localization;
using Plexus.Database.Model.Research;
namespace Plexus.Database.Model
{
	[Table("Employees")]
	public class Employee
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public Guid? AcademicPositionId { get; set; }

		public Guid? CareerPositionId { get; set; }

		public string Code { get; set; }

		public string Title { get; set; }

		public string FirstName { get; set; }

		public string? MiddleName { get; set; }

		public string LastName { get; set; }

		[Column(TypeName = "nvarchar(100)")]
		public Gender Gender { get; set; }

		public string? Country { get; set; }

		public string? Nationality { get; set; }

		public string? Religion { get; set; }

		public string? Race { get; set; }

		public string? CitizenNo { get; set; }

		public string? UniversityEmail { get; set; }

		public string? PersonalEmail { get; set; }

		public string? AlternativeEmail { get; set; }

		public string? PhoneNumber1 { get; set; }

		public string? PhoneNumber2 { get; set; }

		public string? CardImageUrl { get; set; }

		public bool IsActive { get; set; }

		public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		public DateTime UpdatedAt { get; set; }

		public string UpdatedBy { get; set; }

		[ForeignKey(nameof(AcademicPositionId))]
		public virtual AcademicPosition? AcademicPosition { get; set; }

		[ForeignKey(nameof(CareerPositionId))]
		public virtual CareerPosition? CareerPosition { get; set; }

		public virtual EmployeeWorkInformation? WorkInformation { get; set; }

		public virtual IEnumerable<EmployeeEducationalBackground>? EducationalBackgrounds { get; set; }

		public virtual IEnumerable<EmployeeLocalization> Localizations { get; set; }

		public virtual IEnumerable<InstructorAcademicLevel>? AcademicLevels { get; set; }

		public virtual IEnumerable<ResearchCommittee>? ResearchCommittees { get; set; }

		public virtual IEnumerable<StudentTerm> StudentTerms { get; set; }

		public virtual IEnumerable<CourseTopicInstructor> CourseTopics { get; set; }

		//public virtual IEnumerable<AdvisingSlot> AdvisingSlots { get; set; }

	}
}
