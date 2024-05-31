namespace Plexus.Entity.DTO.Payment
{
    public class CreateFeeGroupDTO
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }
    }

    public class FeeGroupDTO : CreateFeeGroupDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}