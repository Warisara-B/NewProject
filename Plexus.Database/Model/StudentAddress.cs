using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;

namespace Plexus.Database.Model
{
	[Table("StudentAddresses")]
	public class StudentAddress
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public Guid StudentId { get; set; }

		[Column(TypeName = "nvarchar(100)")]
		public AddressType Type { get; set; }

		public string? Address1 { get; set; }

		public string? Address2 { get; set; }

		public string? HouseNumber { get; set; }

		public string? Moo { get; set; }

		public string? Soi { get; set; }

		public string? Road { get; set; }

		public string? Province { get; set; }

		public string? District { get; set; }

		public string? SubDistrict { get; set; }

		public string? Country { get; set; }

		public string? PostalCode { get; set; }

		public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		public DateTime UpdatedAt { get; set; }

		public string UpdatedBy { get; set; }

		[ForeignKey(nameof(StudentId))]
		public virtual Student Student { get; set; }
	}
}

