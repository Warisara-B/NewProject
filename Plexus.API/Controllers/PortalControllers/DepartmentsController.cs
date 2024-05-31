using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Client.ViewModel.Academic;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class DepartmentsController : BaseController
    {
        private readonly IDepartmentManager _departmentManager;

        public DepartmentsController(IDepartmentManager departmentManager)
        {
            _departmentManager = departmentManager;
        }

        // TODO: Get user id
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CreateDepartmentViewModel request)
        {
            var department = await _departmentManager.CreateAsync(request, Guid.Empty);

            return StatusCode(201,
                ResponseWrapper.Success(HttpStatusCode.Created, department));
        }

        [HttpGet("search")]
        public IActionResult SearchFaculty([FromQuery] SearchDepartmentCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedDepartment = _departmentManager.Search(parameters, page, pageSize);

            if (pagedDepartment is null || !pagedDepartment.Items.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, pagedDepartment));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var department = _departmentManager.GetById(id);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, department));
        }

        // TODO: Get user id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromForm] UpdateDepartmentViewModel request)
        {
            var department = await _departmentManager.UpdateAsync(id, request, Guid.Empty);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, department));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _departmentManager.Delete(id);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}