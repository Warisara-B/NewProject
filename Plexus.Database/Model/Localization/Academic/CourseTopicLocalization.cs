using System;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Academic;

namespace Plexus.Database.Model.Localization.Academic
{
    [Table("CourseTopics", Schema = "localization")]
	public class CourseTopicLocalization
	{
		public Guid CourseTopicId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

		public string? Name { get; set; }

		public string? Description { get; set; }

        [ForeignKey(nameof(CourseTopicId))]
		public virtual CourseTopic CourseTopic { get; set; }
    }
}

