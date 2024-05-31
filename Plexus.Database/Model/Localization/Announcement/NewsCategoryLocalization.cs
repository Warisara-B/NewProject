using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Announcement;

namespace Plexus.Database.Model.Localization.Announcement
{
    public class NewsCategoryLocalization
	{
		public Guid NewsCategoryId { get; set; }

        public LanguageCode Language { get; set; }

		public string? Name { get; set; }

		[ForeignKey(nameof(NewsCategoryId))]
		public virtual NewsCategory NewsCategory { get; set; }
	}
}

