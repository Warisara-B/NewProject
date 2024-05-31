using System;
namespace Plexus.Entity.DTO.Student
{
	public class PassportDTO
	{
		public string Number { get; set; }

		public DateTime IssuedAt { get; set; }

		public DateTime ExpiredAt { get; set; }

		public string FilePath { get; set; }

		public bool IsActive { get; set; }
	}
}

