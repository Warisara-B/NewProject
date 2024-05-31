using Plexus.Database.Enum;

namespace Plexus.Entity.DTO.Registration
{
    public class RegistrationDTO
    {
        public Guid StudentId { get; set; }

        public Guid TermId { get; set; }

        public RegistrationChannel RegistrationChannel { get; set; }
        
        public IEnumerable<RegistrationSectionDTO> Sections { get; set; }   
   }

   public class RegistrationSectionDTO
   {
        public Guid CourseId { get; set; }
        
        public Guid? SectionId { get; set; }

        public decimal Credit { get; set; }

        public decimal RegistrationCredit { get; set; }

        public Guid? ParentSectionId { get; set; }

        public Guid SectionSeatId { get; set; }

        public bool IsDeductParent { get; set; }
   }
}