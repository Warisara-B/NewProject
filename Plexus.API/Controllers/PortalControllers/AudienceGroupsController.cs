using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel;
using Plexus.Entity.DTO;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class AudienceGroupsController : BaseController
    {
        private readonly IAudienceGroupManager _audienceGroupManager;

        public AudienceGroupsController(IAudienceGroupManager audienceGroupManager)
        {
            _audienceGroupManager = audienceGroupManager;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateAudienceGroupViewModel request)
        {
            var audienceGroup = _audienceGroupManager.Create(request, Guid.Empty);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, audienceGroup));
        }

        [HttpGet("")]
        public IActionResult Search([FromQuery] SearchCriteriaViewModel parameters)
        {
            var audienceGroups = _audienceGroupManager.Search(parameters);

            if (audienceGroups is null || !audienceGroups.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, audienceGroups));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedAudienceGroup = _audienceGroupManager.Search(parameters, page, pageSize);

            if (pagedAudienceGroup is null || !pagedAudienceGroup.Items.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, pagedAudienceGroup));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var audienceGroup = _audienceGroupManager.GetById(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, audienceGroup));
        }

        [HttpPut]
        public IActionResult Update([FromBody] AudienceGroupViewModel request)
        {
            var audienceGroup = _audienceGroupManager.Update(request, Guid.Empty);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, audienceGroup));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _audienceGroupManager.Delete(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}