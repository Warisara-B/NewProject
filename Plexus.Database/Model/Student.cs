using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Academic.Curriculum;
using Plexus.Database.Model.Academic.Faculty;
using Plexus.Database.Model.Localization;
using Plexus.Database.Model.Research;

namespace Plexus.Database.Model
{
	[Table("Students")]
	public class Student
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public string? ProfileImageUrl { get; set; }

		public string Code { get; set; }

		public string Title { get; set; }

		public string FirstName { get; set; }

		public string? MiddleName { get; set; }

		public string LastName { get; set; }

		[Column(TypeName = "nvarchar(100)")]
		public Gender Gender { get; set; }

		public DateTime BirthDate { get; set; }

		public string? BirthCountry { get; set; }

		public string Nationality { get; set; }

		public string Religion { get; set; }

		public string Race { get; set; }

		public string? CitizenId { get; set; }

		public virtual IEnumerable<Passport>? Passports { get; set; }

		public virtual IEnumerable<Deformation>? Deformations { get; set; }

		public virtual IEnumerable<StudentAcademicStatus>? StudentAcademicStatuses { get; set; }

		public string? BankBranch { get; set; }

		public string? BankAccountNo { get; set; }

		public DateTime? BankAccountUpdatedAt { get; set; }

		// public virtual IEnumerable<StudentBankAccount>? BankAccounts { get; set; }

		public string? Remark { get; set; }

		public virtual IEnumerable<StudentAddress>? Addresses { get; set; }

		public string? UniversityEmail { get; set; }

		public string? PersonalEmail { get; set; }

		public string? AlternativeEmail { get; set; }

		public string? Facebook { get; set; }

		public string? Line { get; set; }

		public string? Other { get; set; }

		public string? PhoneNumber1 { get; set; }

		public string? PhoneNumber2 { get; set; }

		public string? PreviousCode { get; set; }

		public Guid? AcademicLevelId { get; set; }

		public int? BatchCode { get; set; }

		public Guid? AcademicProgramId { get; set; }

		public string? CardImageUrl { get; set; }

		public Guid FacultyId { get; set; }

		public Guid? DepartmentId { get; set; }

		public Guid CurriculumVersionId { get; set; }

		public Guid? StudyPlanId { get; set; }

		public decimal? GPA { get; set; }

		public int? CompletedCredit { get; set; }

		public int? RegistrationCredit { get; set; }

		public int? TransferredCredit { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }

		public bool IsActive { get; set; }

		public virtual IEnumerable<StudentLocalization> Localizations { get; set; }

		public virtual IEnumerable<StudentGuardian>? Guardians { get; set; }

		public virtual IEnumerable<StudentTerm>? StudentTerms { get; set; }

		public virtual IEnumerable<AcademicSpecialization>? AcademicSpecializations { get; set; }

		public virtual IEnumerable<CourseTrack> CourseTracks { get; set; }

		public virtual IEnumerable<ResearchMember>? Researches { get; set; }

		[ForeignKey(nameof(AcademicLevelId))]
		public virtual AcademicLevel? AcademicLevel { get; set; }

		[ForeignKey(nameof(AcademicProgramId))]
		public virtual AcademicProgram? AcademicProgram { get; set; }

		[ForeignKey(nameof(FacultyId))]
		public virtual Faculty Faculty { get; set; }

		[ForeignKey(nameof(DepartmentId))]
		public virtual Department? Department { get; set; }

		[ForeignKey(nameof(CurriculumVersionId))]
		public virtual CurriculumVersion CurriculumVersion { get; set; }

		[ForeignKey(nameof(StudyPlanId))]
		public virtual StudyPlan? StudyPlan { get; set; }
	}
}

