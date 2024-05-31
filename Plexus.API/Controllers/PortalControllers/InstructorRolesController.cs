using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Utility;

namespace Plexus.API.Controllers.PortalControllers
{
    [ApiController]

    [Route(_pathPrefix + "[controller]")]
    public class InstructorRolesController : BaseController
    {
        private readonly IInstructorRoleManager _instructorRoleManager;

        public InstructorRolesController(IInstructorRoleManager instructorRoleManager)
        {
            _instructorRoleManager = instructorRoleManager;
        }

        [HttpPost]
        public IActionResult Create(CreateInstructorRoleViewModel request)
        {
            var instructorRole = _instructorRoleManager.Create(request, Guid.Empty);

            return StatusCode(201,
                ResponseWrapper.Success(HttpStatusCode.Created, instructorRole));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchInstructorRoleCriteriaViewModel parameters, int page = 1, int pageSize = 5)
        {
            var pagedInstructorRole = _instructorRoleManager.Search(parameters, page, pageSize);

            if (pagedInstructorRole is null || !pagedInstructorRole.Items.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, pagedInstructorRole));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var instructorRole = _instructorRoleManager.GetById(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, instructorRole));
        }


        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] CreateInstructorRoleViewModel request)
        {
            var instructorRole = _instructorRoleManager.Update(id, request, Guid.Empty);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, instructorRole));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _instructorRoleManager.Delete(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}