using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Announcement;

namespace Plexus.Database.Model.Localization.Announcement
{
    public class NewsLocalization
	{
		public Guid NewsId { get; set; }

		public LanguageCode Language { get; set; }

		public string? Title { get; set; }

		public string? Content { get; set; }

		[ForeignKey(nameof(NewsId))]
		public virtual News News { get; set; }
	}
}

