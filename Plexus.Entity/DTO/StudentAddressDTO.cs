using Plexus.Database.Enum;

namespace Plexus.Entity.DTO
{
    public class CreateStudentAddressDTO
    {
        public Guid StudentId { get; set; }

        public AddressType Type { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string? HouseNumber { get; set; }

        public string? Moo { get; set; }

        public string? Soi { get; set; }

        public string? Road { get; set; }

        public string? Province { get; set; }

        public string? District { get; set; }

        public string? SubDistrict { get; set; }

        public string? Country { get; set; }

        public string? PostalCode { get; set; }
    }

    public class StudentAddressDTO : CreateStudentAddressDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}