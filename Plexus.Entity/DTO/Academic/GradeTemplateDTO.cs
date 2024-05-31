using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plexus.Entity.DTO.Academic
{

    public class CreateGradeTemplateDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
    public class GradeTemplateDTO : CreateGradeTemplateDTO
    {
        public Guid Id { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; } 
    }
}
