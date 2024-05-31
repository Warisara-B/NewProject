using System;
namespace Plexus.Database.Enum.Student
{
	[Flags]
	public enum AdmissionType
	{
		ENTRANCE = 1,
		DIRECT_ENTRANCE = 2,
		INTERNATIONAL_EXCHANGE = 4,
		RE_ENTER = 8
	}
}

