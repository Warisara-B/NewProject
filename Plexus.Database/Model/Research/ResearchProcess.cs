using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum.Research;
using Plexus.Database.Model.Localization.Research;

namespace Plexus.Database.Model.Research
{
    [Table("ResearchProcesses")]
    public class ResearchProcess
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid ResearchTemplateId { get; set; }

        public Guid AppointmentId { get; set; }

        public string? Name { get; set; }

        public string? FilePrefix { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public ResearchSequenceType Type { get; set; }

        public bool? IsCompleted { get; set; }

        public DateTime? CompletedAt { get; set; }

        public DateTime? AppointedDate { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public ResearchAppointmentType? AppointmentType { get; set; }

        public bool? HasCommitteeConfirmed { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public ResearchDefenseStatus DefenseStatus { get; set; }

        public string? Remark { get; set; }

        public DateTime DueDate { get; set; }

        public int? NotificationDay { get; set; }

        [ForeignKey(nameof(ResearchTemplateId))]
        public ResearchTemplate? ResearchTemplate { get; set; }

        public virtual IEnumerable<ResearchProcessLocalization>? Localizations { get; set; }

        public virtual IEnumerable<ResearchResource>? Resources { get; set; }

        public virtual IEnumerable<ResearchCommittee>? Committees { get; set; }
    }
}