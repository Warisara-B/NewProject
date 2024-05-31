using System;
namespace Plexus.Entity.DTO.Registration
{
	public class CreatePlanScheduleDTO
    {
		public IEnumerable<Guid> SectionIds { get; set; }
	}

	public class PlanScheduleDTO : CreatePlanScheduleDTO
	{
		public Guid Id { get; set; }
	}
}

