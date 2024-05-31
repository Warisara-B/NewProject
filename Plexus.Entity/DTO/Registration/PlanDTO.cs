using System;
using Plexus.Database.Enum;

namespace Plexus.Entity.DTO.Registration
{
	public class CreatePlanDTO
	{
		public PlanType Type { get; set; }

		public IEnumerable<Guid> CourseIds { get; set; }
	}

	public class PlanDTO : CreatePlanDTO
	{
		public Guid Id { get; set; }
	}
}

