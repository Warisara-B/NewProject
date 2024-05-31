using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Academic;

namespace Plexus.Database.Model.Localization.Academic
{
    [Table("CourseTracks", Schema = "localization")]
	public class CourseTrackLocalization
	{
		public Guid CourseTrackId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

		public string? Name { get; set; }

        [ForeignKey(nameof(CourseTrackId))]
		public virtual CourseTrack CourseTrack { get; set; }
	}
}