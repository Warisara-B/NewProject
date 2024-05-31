using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;
using Plexus.Entity.Utilities;

namespace Plexus.Client.ViewModel.Academic
{
    public class CreateCourseTopicViewModel
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonIgnore]
        public string? Name => Localizations?.GetDefault().Name;

        [JsonIgnore]
        public string? Description => Localizations?.GetDefault().Description;

        [JsonProperty("lectureHour")]
        public decimal LectureHour { get; set; }

        [JsonProperty("labHour")]
        public decimal LabHour { get; set; }

        [JsonProperty("otherHour")]
        public decimal OtherHour { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("instructorIds")]
        public IEnumerable<Guid>? InstructorIds { get; set; }

        [JsonProperty("localizations")]
        public IEnumerable<CourseTopicLocalizationViewModel>? Localizations { get; set; }
    }

    public class CourseTopicViewModel : CreateCourseTopicViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("courseId")]
        public Guid CourseId { get; set; }

        [JsonProperty("courseName")]
        public string? CourseName { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("name")]
        public new string Name { get { return base.Name; } }

        [JsonProperty("description")]
        public new string Description { get { return base.Description; } }

        [JsonIgnore]
        public new IEnumerable<Guid>? InstructorIds { get; set; }

        [JsonProperty("instructors")]
        public IEnumerable<CourseTopicInstructorViewModel> Instructors { get; set; }
    }

    public class CourseTopicLocalizationViewModel
    {
        [JsonProperty("language")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(LanguageCode))]
        public LanguageCode Language { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }
    }

    public class CourseTopicInstructorViewModel
    {
        [JsonProperty("instructorId")]
        public Guid InstructorId { get; set; }

        [JsonProperty("instructorFirstName")]
        public string? InstructorFirstName { get; set; }

        [JsonProperty("instructorMiddleName")]
        public string? InstructorMiddleName { get; set; }

        [JsonProperty("instructorLastName")]
        public string? InstructorLastName { get; set; }
    }
}

