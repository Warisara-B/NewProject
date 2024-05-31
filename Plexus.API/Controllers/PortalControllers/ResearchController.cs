using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.Research;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Utility;

namespace Plexus.API.Controllers.PortalControllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class ResearchController : BaseController
	{
        private readonly IResearchTemplateManager _researchTemplateManager;

        public ResearchController(IResearchTemplateManager researchTemplateManager)
        {
            _researchTemplateManager = researchTemplateManager;
        }

        [HttpPost("template")]
        public IActionResult CreateTemplate([FromBody] UpsertResearchTemplateViewModel request)
        {
            var researchTemplate = _researchTemplateManager.Create(request, Guid.Empty);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, researchTemplate));
        }

        [HttpGet("template/search")]
        public IActionResult SearchTemplate([FromQuery] SearchResearchTemplateCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedResearchTemplate = _researchTemplateManager.Search(parameters, page, pageSize);
            if (pagedResearchTemplate is null || !pagedResearchTemplate.Items.Any())
            {
                return NoContent();
            }
            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, pagedResearchTemplate));
        }

        [HttpGet("template/{id}")]
        public IActionResult GetTemplateById(Guid id)
        {
            var researchTemplate = _researchTemplateManager.GetById(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, researchTemplate));
        }

        [HttpPut("template/{id}")]
        public IActionResult UpdateTemplate(Guid id, [FromBody] UpsertResearchTemplateViewModel request)
        {
            var researchTemplate = _researchTemplateManager.Update(id, request, Guid.Empty);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, researchTemplate));
        }

        [HttpDelete("template/{id}")]
        public IActionResult DeleteTemplate(Guid id)
        {
            _researchTemplateManager.Delete(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}

