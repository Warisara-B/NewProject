using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model
{
	[Table("Passports")]
	public class Passport
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public Guid StudentId { get; set; }

		public string Number { get; set; }

		public DateTime IssuedAt { get; set; }

		public DateTime ExpiredAt { get; set; }

		/// <summary>
		/// File path upload to Cloud storage (Azure or ETC.)
		/// </summary>
		public string FilePath { get; set; }

		public bool IsActive { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }

		[ForeignKey(nameof(StudentId))]
		public virtual Student Student { get; set; }
	}
}

