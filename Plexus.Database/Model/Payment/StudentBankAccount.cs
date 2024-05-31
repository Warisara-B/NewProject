using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Payment
{
	[Table("StudentBankAccounts")]
	public class StudentBankAccount
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public Guid StudentId { get; set; }

		public Guid BankId { get; set; }

		public string AccountHolderName { get; set; }

		public string AccountNumber { get; set; }

		public bool IsActive { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }

		[ForeignKey(nameof(StudentId))]
		public virtual Student Student { get; set; }

		[ForeignKey(nameof(BankId))]
		public virtual Bank Bank { get; set; }
	}
}

