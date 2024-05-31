using System;
using Plexus.Database.Enum.Academic.Section;

namespace Plexus.Entity.DTO.Academic.Section
{
    public class UpdateSectionDetailDTO
    {
        public DayOfWeek Day { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public Guid? RoomId { get; set; }

        public Guid? InstructorId { get; set; }

        public Guid? TeachingTypeId { get; set; }

        public string? Remark { get; set; }
    }

    public class SectionDetailDTO : UpdateSectionDetailDTO
    {
        public Guid Id { get; set; }

        public Guid SectionId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

