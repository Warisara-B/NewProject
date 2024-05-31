using Plexus.Database.Enum.Academic;

namespace Plexus.Entity.DTO.Academic.Section
{
    public class UpdateSectionExaminationDTO
    {
        public ExamType ExamType { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan StartTime { get; set; }

		public TimeSpan EndTime { get; set; }

        public Guid? RoomId { get; set; }
    }

    public class SectionExaminationDTO : UpdateSectionExaminationDTO
    {
        public Guid Id { get; set; }

        public Guid SectionId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}