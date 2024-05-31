using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Plexus.Utility.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Plexus.Entity.DTO
{
    public class SearchGradeTemplateViewModel
    {        
        [FromQuery(Name = "Name")]
        public string? Name { get; set; }

        [FromQuery(Name = "Description")]
        public string? Description { get; set; }
        
        [FromQuery(Name = "IsActive")]       
        public bool? IsActive { get; set; }

        [FromQuery(Name = "createdAt")]
        public DateTime? CreatedAt { get; set; }

        [FromQuery(Name = "updatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [FromQuery(Name = "sortBy")]
        public string? SortBy { get; set; }

        [FromQuery(Name = "orderBy")]
        public SortingOrder? OrderBy { get; set; } = SortingOrder.ASC;

    }
}
