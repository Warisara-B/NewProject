using System;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Academic;

namespace Plexus.Database.Model.Localization.Academic
{
    [Table("Courses", Schema = "localization")]
	public class CourseLocalization
	{
		public Guid CourseId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

		public string? Name { get; set; }

		public string? TranscriptName1 { get; set; }

        public string? TranscriptName2 { get; set; }

        public string? TranscriptName3 { get; set; }

		public string? Description { get; set; }

        [ForeignKey(nameof(CourseId))]
		public virtual Course Course { get; set; }
    }
}

