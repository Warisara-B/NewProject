using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Model.Localization.Announcement;

namespace Plexus.Database.Model.Announcement
{
    public class News
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public Guid CategoryId { get; set; }

		public Guid PublisherId { get; set; }

		public string Title { get; set; }

		public string Content { get; set; }

		public bool IsPinned { get; set; }

		public string? ThumbnailUrl { get; set; }

		public string? BannerUrl { get; set; }

		public DateTime PublishAt { get; set; }

		public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		public DateTime UpdatedAt { get; set; }

		public string UpdatedBy { get; set; }

		[ForeignKey(nameof(CategoryId))]
		public virtual NewsCategory NewsCategory { get; set; }

        [ForeignKey(nameof(PublisherId))]
        public virtual Publisher Publisher { get; set; }

		public virtual IEnumerable<NewsLocalization> Localizations { get; set; }
    }
}

