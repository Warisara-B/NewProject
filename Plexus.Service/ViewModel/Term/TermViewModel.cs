using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;
using Plexus.Database.Enum.Academic;
using Plexus.Database.Model.Academic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plexus.Service.ViewModel.Term
{
    public class TermViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("termType")]
        [EnumDataType(typeof(TermType))]
        [JsonConverter(typeof(StringEnumConverter))]
        public TermType TermType { get; set; }

        [JsonProperty("collegeCalendarType")]
        [EnumDataType(typeof(CollegeCalendarType))]
        [JsonConverter(typeof(StringEnumConverter))]
        public CollegeCalendarType CollegeCalendarType { get; set; }

        [JsonProperty("startedAt")]
        public DateTime StartedAt { get; set; }

        [JsonProperty("endedAt")]
        public DateTime EndedAt { get; set; }

        [JsonProperty("totalWeeks")]
        public int TotalWeeks { get; set; }

        [JsonProperty("isCurrent")]
        public bool IsCurrent { get; set; }

        [JsonProperty("isStudentRegistration")]
        public bool IsStudentRegistration { get; set; }

        [JsonProperty("academicLevel")]
        public AcademicLevelObj AcademicLevel { get; set; }
    }

    public class AcademicLevelObj
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("formalName")]
        public string FormalName { get; set; }
    }
}
