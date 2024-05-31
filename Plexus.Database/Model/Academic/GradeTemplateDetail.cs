using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Academic
{
    [Table("GradeTemplateDetails")]
    public class GradeTemplateDetail
    {
        public Guid GradeTemplateId { get; set; }
        public Guid GradeId { get; set; }

        [ForeignKey(nameof(GradeTemplateId))]
        public virtual GradeTemplate GradeTemplate { get; set; }

        [ForeignKey(nameof(GradeId))]
        public virtual Grade Grade { get; set; }
    }
}

