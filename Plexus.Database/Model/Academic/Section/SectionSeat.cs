using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum.Academic.Section;

namespace Plexus.Database.Model.Academic
{
    [Table("SectionSeats")]
    public class SectionSeat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid SectionId { get; set; }

        public string Name { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public SeatType Type { get; set; }

        public string? Conditions { get; set; }

        public int TotalSeat { get; set; }

        public int SeatUsed { get; set; }

        public string? Remark { get; set; }

        public Guid? MasterSeatId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        [ForeignKey(nameof(SectionId))]
        public virtual Section.Section Section { get; set; }

        [ForeignKey(nameof(MasterSeatId))]
        public virtual SectionSeat? MasterSeat { get; set; }
    }
}

