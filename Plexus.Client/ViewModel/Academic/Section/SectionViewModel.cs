using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Client.ViewModel.Academic.Section;
using Plexus.Database.Enum.Academic;
using Plexus.Database.Enum.Academic.Section;

namespace Plexus.Client.ViewModel.Academic
{
    #region Request

    public class CreateSectionViewModel
    {
        [JsonProperty("academicLevelId")]
        public Guid AcademicLevelId { get; set; }

        [JsonProperty("termId")]
        public Guid TermId { get; set; }

        [JsonProperty("courseId")]
        public Guid CourseId { get; set; }

        [JsonProperty("campusId")]
        public Guid CampusId { get; set; }

        [JsonProperty("sectionNo")]
        public string SectionNo { get; set; }

        [JsonProperty("seatLimit")]
        public int SeatLimit { get; set; }

        [JsonProperty("availableSeat")]
        public int? AvailableSeat { get; set; }

        [JsonProperty("instructorIds")]
        public IEnumerable<Guid> InstructorIds { get; set; }

        [JsonProperty("isWithdrawable")]
        public bool IsWithdrawable { get; set; }

        [JsonProperty("isInvisibled")]
        public bool IsInvisibled { get; set; }

        [JsonProperty("isClosed")]
        public bool IsClosed { get; set; }

        [JsonProperty("remark")]
        public string? Remark { get; set; }

        [JsonProperty("facultyId")]
        public Guid? FacultyId { get; set; }

        [JsonProperty("departmentId")]
        public Guid? DepartmentId { get; set; }

        [JsonProperty("curriculumVersionId")]
        public Guid? CurriculumVersionId { get; set; }

        [JsonProperty("batch")]
        public string? Batch { get; set; }

        [JsonProperty("studentCodes")]
        public string? StudentCodes { get; set; }

        [JsonProperty("jointSections")]
        public IEnumerable<CreateJointSectionViewModel>? JointSections { get; set; }

        [JsonProperty("startedAt")]
        public DateTime StartedAt { get; set; }

        [JsonProperty("endedAt")]
        public DateTime EndedAt { get; set; }

        [JsonProperty("classPeriods")]
        public IEnumerable<CreateSectionClassPeriodViewModel> ClassPeriods { get; set; }

        [JsonProperty("examination1")]
        public CreateSectionExaminationViewModel Examination1 { get; set; }

        [JsonProperty("examination2")]
        public CreateSectionExaminationViewModel Examination2 { get; set; }
    }

    public class CreateJointSectionViewModel
    {
        [JsonProperty("parentSectionId")]
        public Guid ParentSectionId { get; set; }

        [JsonProperty("courseId")]
        public Guid CourseId { get; set; }

        [JsonProperty("sectionNo")]
        public string SectionNo { get; set; }

        [JsonProperty("seatLimit")]
        public int SeatLimit { get; set; }

        [JsonProperty("availableSeat")]
        public int? AvailableSeat { get; set; }

        [JsonProperty("seatUsed")]
        public int? SeatUsed { get; set; }

        [JsonProperty("remark")]
        public string? Remark { get; set; }
    }

    public class CreateSectionClassPeriodViewModel
    {
        [JsonProperty("roomId")]
        public Guid? RoomId { get; set; }

        [JsonProperty("day")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(DayOfWeek))]
        public DayOfWeek Day { get; set; }

        [JsonProperty("startTime")]
        public TimeSpan StartTime { get; set; }

        [JsonProperty("endTime")]
        public TimeSpan EndTime { get; set; }

        [JsonProperty("instructorIds")]
        public IEnumerable<Guid> InstructorIds { get; set; }
    }

    public class CreateSectionExaminationViewModel
    {
        [JsonProperty("roomId")]
        public Guid? RoomId { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(ExamType))]
        public ExamType? Type { get; set; }

        [JsonProperty("date")]
        public DateTime? Date { get; set; }

        [JsonProperty("startTime")]
        public TimeSpan? StartTime { get; set; }

        [JsonProperty("endTime")]
        public TimeSpan? EndTime { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }

    #endregion
    #region Response

    public class SectionViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("academicLevelId")]
        public Guid AcademicLevelId { get; set; }

        [JsonProperty("academicLevelName")]
        public string? AcademicLevelName { get; set; }

        [JsonProperty("termId")]
        public Guid TermId { get; set; }

        [JsonProperty("termName")]
        public string? TermName { get; set; }

        [JsonProperty("courseId")]
        public Guid CourseId { get; set; }

        [JsonProperty("courseCode")]
        public string? CourseCode { get; set; }

        [JsonProperty("courseName")]
        public string? CourseName { get; set; }

        [JsonProperty("campusId")]
        public Guid CampusId { get; set; }

        [JsonProperty("campusName")]
        public string? CampusName { get; set; }

        [JsonProperty("sectionNo")]
        public string SectionNo { get; set; }

        [JsonProperty("seatLimit")]
        public int SeatLimit { get; set; }

        [JsonProperty("availableSeat")]
        public int? AvailableSeat { get; set; }

        [JsonProperty("seatUsed")]
        public int? SeatUsed { get; set; }

        [JsonProperty("status")]
        public SectionStatus Status { get; set; }

        [JsonProperty("instructorIds")]
        public IEnumerable<SectionInstructorViewModel>? Instructors { get; set; }

        [JsonProperty("isWithdrawable")]
        public bool IsWithdrawable { get; set; }

        [JsonProperty("isInvisibled")]
        public bool IsInvisibled { get; set; }

        [JsonProperty("isClosed")]
        public bool IsClosed { get; set; }

        [JsonProperty("remark")]
        public string? Remark { get; set; }

        [JsonProperty("facultyId")]
        public Guid? FacultyId { get; set; }

        [JsonProperty("facultyName")]
        public string? FacultyName { get; set; }

        [JsonProperty("departmentId")]
        public Guid? DepartmentId { get; set; }

        [JsonProperty("departmentName")]
        public string? DepartmentName { get; set; }

        [JsonProperty("curriculumVersionId")]
        public Guid? CurriculumVersionId { get; set; }

        [JsonProperty("curriculumVersionName")]
        public string? CurriculumVersionName { get; set; }

        [JsonProperty("batch")]
        public string? Batch { get; set; }

        [JsonProperty("studentCodes")]
        public string? StudentCodes { get; set; }

        [JsonProperty("jointSections")]
        public IEnumerable<JointSectionViewModel>? JointSections { get; set; }

        [JsonProperty("startedAt")]
        public DateTime? StartedAt { get; set; }

        [JsonProperty("endedAt")]
        public DateTime? EndedAt { get; set; }

        [JsonProperty("classPeriods")]
        public IEnumerable<SectionClassPeriodViewModel> ClassPeriods { get; set; }

        [JsonProperty("examinations")]
        public IEnumerable<SectionExaminationViewModel> Examinations { get; set; }
    }

    public class SectionInstructorViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("firstName")]
        public string? FirstName { get; set; }

        [JsonProperty("middleName")]
        public string? MiddleName { get; set; }

        [JsonProperty("lastName")]
        public string? LastName { get; set; }
    }

    public class JointSectionViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("parentSectionId")]
        public Guid ParentSectionId { get; set; }

        [JsonProperty("type")]
        public SectionType Type { get; set; }

        [JsonProperty("courseId")]
        public Guid CourseId { get; set; }

        [JsonProperty("courseCode")]
        public string? CourseCode { get; set; }

        [JsonProperty("courseName")]
        public string? CourseName { get; set; }

        [JsonProperty("sectionNo")]
        public string SectionNo { get; set; }

        [JsonProperty("seatLimit")]
        public int SeatLimit { get; set; }

        [JsonProperty("availableSeat")]
        public int? AvailableSeat { get; set; }

        [JsonProperty("seatUsed")]
        public int? SeatUsed { get; set; }

        [JsonProperty("remark")]
        public string? Remark { get; set; }
    }

    public class SectionClassPeriodViewModel : CreateSectionClassPeriodViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("roomName")]
        public string? RoomName { get; set; }

        [JsonProperty("instructorNames")]
        public IEnumerable<SectionInstructorViewModel> InstructorNames { get; set; }
    }

    public class SectionExaminationViewModel : CreateSectionExaminationViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("roomName")]
        public string? RoomName { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }
    }

    #endregion
}