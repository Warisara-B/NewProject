using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Payment
{
	[Table("Banks")]
	public class Bank
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public string Code { get; set; }

		public string Name { get; set; }

		public string IconFilePath { get; set; }

		public bool IsActive { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }
	}
}

