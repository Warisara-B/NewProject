namespace Plexus.Entity.DTO.Academic.Section
{
    public class CreateOfferedCourseDTO : CreateSectionDTO
    {
        public IEnumerable<UpsertSectionSeatDTO>? Seats { get; set; }

        public IEnumerable<UpdateSectionDetailDTO> Details { get; set; }

        public IEnumerable<UpdateSectionExaminationDTO> Examinations { get; set; }
    }

    public class CreateJointSectionDTO
    {
        public Guid CourseId { get; set; }

        public string Number { get; set; }

        public int SeatLimit { get; set; }

        public string? Remark { get; set; }
    }

    public class JointSectionDTO : CreateJointSectionDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }
    }

    public class OfferedCourseDTO : SectionDTO
    {
        public IEnumerable<SectionSeatDTO>? Seats { get; set; }

        public IEnumerable<JointSectionDTO>? JointSections { get; set; }

        public IEnumerable<SectionDetailDTO> Details { get; set; }

        public IEnumerable<SectionExaminationDTO> Examinations { get; set; }
    }
}