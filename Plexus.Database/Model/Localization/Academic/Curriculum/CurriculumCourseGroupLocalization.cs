using System;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Academic.Curriculum;

namespace Plexus.Database.Model.Localization.Academic.Curriculum
{
    [Table("CurriculumCourseGroups", Schema = "localization")]
    public class CurriculumCourseGroupLocalization
    {
        public Guid CurriculumCourseGroupId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        [ForeignKey(nameof(CurriculumCourseGroupId))]
        public virtual CurriculumCourseGroup CurriculumCourseGroup { get; set; }
    }
}

