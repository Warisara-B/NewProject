namespace Plexus.Entity.DTO.Payment.Scholarship
{
    public class CreateScholarshipTypeDTO
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }
    }

    public class ScholarshipTypeDTO : CreateScholarshipTypeDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}