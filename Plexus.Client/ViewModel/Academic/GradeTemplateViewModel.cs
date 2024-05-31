using System;
using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.Academic
{
    public class CreateGradeTemplateViewModel {

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("IsActive")]
        public bool IsActive { get; set; }
    }
    public class GradeTemplateViewModel : CreateGradeTemplateViewModel { 
    
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

    }

}
