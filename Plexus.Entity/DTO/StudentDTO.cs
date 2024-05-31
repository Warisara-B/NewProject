using Plexus.Database.Enum;
using Plexus.Database.Enum.Student;
using Plexus.Entity.DTO.Student;

namespace Plexus.Entity.DTO
{
	public class CreateStudentDTO
	{
		public Guid AcademicLevelId { get; set; }

		public Guid FacultyId { get; set; }

		public Guid? DepartmentId { get; set; }

		public Guid CurriculumVersionId { get; set; }

		public Guid? StudentFeeTypeId { get; set; }

		public string Code { get; set; }

		public string Title { get; set; }

		public string FirstName { get; set; }

		public string? MiddleName { get; set; }

		public string LastName { get; set; }

		public Gender Gender { get; set; }

		public DateTime BirthDate { get; set; }

		public string? BirthCountry { get; set; }

		public string Nationality { get; set; }

		public string Religion { get; set; }

		public string Race { get; set; }

		public string? CitizenId { get; set; }

		public virtual IEnumerable<PassportDTO>? Passports { get; set; }

		public virtual IEnumerable<DeformationDTO>? Deformations { get; set; }

		public AcademicStatus StudentStatus { get; set; }

		public DateTime StudentStatusEffectiveDate { get; set; }

		public string? StudentStatusRemark { get; set; }

		public string? BankBranch { get; set; }

		public string? BankAccountNo { get; set; }

		public DateTime? BankAccountUpdatedAt { get; set; }

		public string? Remark { get; set; }

		public string? UniversityEmail { get; set; }

		public string? PersonalEmail { get; set; }

		public string? AlternativeEmail { get; set; }

		public string? Facebook { get; set; }

		public string? Line { get; set; }

		public string? Other { get; set; }

		public string? PhoneNumber1 { get; set; }

		public string? PhoneNumber2 { get; set; }

		public int BatchCode { get; set; }

		public int? GPA { get; set; }

		public IEnumerable<StudentLocalizationDTO>? Localizations { get; set; }
	}

	public class StudentDTO : CreateStudentDTO
	{
		public Guid Id { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }
	}

	public class StudentLocalizationDTO
	{
		public LanguageCode Language { get; set; }

		public string? FirstName { get; set; }

		public string? MiddleName { get; set; }

		public string? LastName { get; set; }
	}

	public class UpdateStudentGeneralInfoDTO
	{
		public string Title { get; set; }

		public string FirstName { get; set; }

		public string? MiddleName { get; set; }

		public string LastName { get; set; }

		public Gender Gender { get; set; }

		public DateTime BirthDate { get; set; }

		public string? BirthCountry { get; set; }

		public string Nationality { get; set; }

		public string Religion { get; set; }

		public string Race { get; set; }

		public string? CitizenId { get; set; }

		public virtual IEnumerable<PassportDTO>? Passports { get; set; }

		public virtual IEnumerable<DeformationDTO>? Deformations { get; set; }

		public AcademicStatus StudentStatus { get; set; }

		public DateTime StudentStatusEffectiveDate { get; set; }

		public string? StudentStatusRemark { get; set; }

		public string? BankBranch { get; set; }

		public string? BankAccountNo { get; set; }

		public DateTime? BankAccountUpdatedAt { get; set; }

		public string? Remark { get; set; }

		public IEnumerable<StudentLocalizationDTO> Localizations { get; set; }
	}

	public class StudentGeneralInfoDTO : UpdateStudentGeneralInfoDTO
	{
		public Guid Id { get; set; }
	}
}

