using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Payment;

namespace Plexus.Database.Model.Localization.Payment
{
    [Table("FeeItems", Schema = "localization")]
	public class FeeItemLocalization
	{
		public Guid FeeItemId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

		public string? Name { get; set; }

        [ForeignKey(nameof(FeeItemId))]
		public virtual FeeItem FeeItem { get; set; }
	}
}