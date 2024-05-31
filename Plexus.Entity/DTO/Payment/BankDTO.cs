using System;
namespace Plexus.Entity.DTO.Payment
{
	public class BankDTO : CreateBankDTO
	{
		public Guid Id { get; set; }

		public bool IsActive { get; set; }
	}

	public class CreateBankDTO
    {
		public string Code { get; set; }

		public string Name { get; set; }

		public string IconImageFilePath { get; set; }
	}
}

