using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum.Payment;

namespace Plexus.Database.Model.Payment
{
	[Table("TermFeePackages")]
	public class TermFeePackage
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public string Name { get; set; }

		[Column(TypeName = "nvarchar(100)")]
		public TermFeePackageType Type { get; set; }

		public bool IsActive { get; set; }

		public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		public DateTime UpdatedAt { get; set; }

		public string UpdatedBy { get; set; }

		public virtual IEnumerable<TermFeeItem> Items { get; set; }
	}
}