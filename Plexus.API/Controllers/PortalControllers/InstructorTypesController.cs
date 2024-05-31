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
    public class InstructorTypesController : BaseController
    {
        private readonly IInstructorTypeManager _instructorTypeManager;

        public InstructorTypesController(IInstructorTypeManager instructorTypeManager)
        {
            _instructorTypeManager = instructorTypeManager;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateInstructorTypeViewModel request)
        {
            var instructorType = _instructorTypeManager.Create(request, Guid.Empty);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, instructorType));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedInstructorType = _instructorTypeManager.Search(parameters, page, pageSize);

            if (pagedInstructorType is null || !pagedInstructorType.Items.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, pagedInstructorType));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var instructorType = _instructorTypeManager.GetById(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, instructorType));
        }

        [HttpPut]
		public IActionResult Update([FromBody] InstructorTypeViewModel request)
        {
			var instructorType = _instructorTypeManager.Update(request, Guid.Empty);

			return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, instructorType));
        }

        [HttpDelete("{id}")]
		public IActionResult Delete(Guid id)
		{
			_instructorTypeManager.Delete(id);

			return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
		}
    }
}