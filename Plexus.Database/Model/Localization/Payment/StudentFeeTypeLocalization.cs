using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Payment;

namespace Plexus.Database.Model.Localization.Payment
{
    [Table("StudentFeeTypes", Schema = "localization")]
	public class StudentFeeTypeLocalization
	{
		public Guid StudentFeeTypeId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

		public string? Name { get; set; }

        [ForeignKey(nameof(StudentFeeTypeId))]
		public virtual StudentFeeType StudentFeeType { get; set; }
	}
}