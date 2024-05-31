using System;
namespace Plexus.Entity.DTO.Payment
{
	public class StudentBankAccountDTO : CreateStudentBankAccountDTO
	{
		public string BankName { get; set; }

		public string BankCode { get; set; }

		public string BankIconImageFilePath { get; set; }

		public bool IsActive { get; set; }
	}

	public class CreateStudentBankAccountDTO
	{
		public Guid BankId { get; set; }

		public Guid StudentId { get; set; }

		public string AccountHolderName { get; set; }

		public string AccountNumber { get; set; }
	}
}

