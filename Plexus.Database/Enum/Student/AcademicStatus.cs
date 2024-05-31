using System;
namespace Plexus.Database.Enum.Student
{
	[Flags]
	public enum AcademicStatus
	{
		STUDYING,
		GRADUATED,
		BLACKLISTED
		// REGISTERED = 1,
		// REJECTED = 2,
		// STUDYING = 4,
		// GRADUATING = 8,
		// GRADUATED = 16,
		// SUSPENDED = 32,
		// EXPELLED = 64,
		// RETIRED = 128
	}
}

