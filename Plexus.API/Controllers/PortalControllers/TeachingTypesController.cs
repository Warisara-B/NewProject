using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class TeachingTypesController : BaseController
    {
        private readonly ITeachingTypeManager _teachingTypeManager;

        public TeachingTypesController(ITeachingTypeManager teachingTypeManager)
        {
            _teachingTypeManager = teachingTypeManager;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateTeachingTypeViewModel request)
        {
            var teachingType = _teachingTypeManager.Create(request, Guid.Empty);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, teachingType));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchTeachingTypeCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedTeachingType = _teachingTypeManager.Search(parameters, page, pageSize);

            if (pagedTeachingType is null || !pagedTeachingType.Items.Any())
            {
                return NoContent();
            }

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, pagedTeachingType));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var teachingType = _teachingTypeManager.GetById(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, teachingType));
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] CreateTeachingTypeViewModel request)
        {
            var teachingType = _teachingTypeManager.Update(id, request, Guid.Empty);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, teachingType));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _teachingTypeManager.Delete(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}