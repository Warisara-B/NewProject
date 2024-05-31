using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Entity.DTO;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class FacultiesController : BaseController
    {
        private readonly IFacultyManager _facultyManager;

        public FacultiesController(IFacultyManager facultyManager)
        {
            _facultyManager = facultyManager;
        }

        // TODO: Get user id
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CreateFacultyViewModel request)
        {
            var faculty = await _facultyManager.CreateAsync(request, Guid.Empty);

            return StatusCode(201,
                ResponseWrapper.Success(HttpStatusCode.Created, faculty));
        }

        [HttpGet("search")]
        public IActionResult SearchFaculty([FromQuery] SearchFacultyCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedFaculty = _facultyManager.Search(parameters, page, pageSize);

            if (pagedFaculty is null || !pagedFaculty.Items.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, pagedFaculty));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var faculty = _facultyManager.GetById(id);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, faculty));
        }

        // TODO: Get user id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromForm] UpdateFacultyViewModel request)
        {
            var faculty = await _facultyManager.UpdateAsync(id, request, Guid.Empty);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, faculty));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _facultyManager.Delete(id);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}