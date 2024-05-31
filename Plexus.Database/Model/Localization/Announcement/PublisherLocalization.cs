using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Announcement;

namespace Plexus.Database.Model.Localization.Announcement
{
    public class PublisherLocalization
	{
		public Guid PublisherId { get; set; }

		public LanguageCode Language { get; set; }

		public string Name { get; set; }

		[ForeignKey(nameof(PublisherId))]
		public virtual Publisher Publisher { get; set; }
	}
}

