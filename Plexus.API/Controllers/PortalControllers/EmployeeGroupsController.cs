using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class EmployeeGroupsController : BaseController
    {
        private readonly IEmployeeGroupManager _employeeGroupManager;

        public EmployeeGroupsController(IEmployeeGroupManager employeeGroupManager)
        {
            _employeeGroupManager = employeeGroupManager;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateEmployeeGroupViewModel request)
        {
            var employeeGroup = _employeeGroupManager.Create(request, Guid.Empty);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, employeeGroup));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchEmployeeGroupCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedemployeeGroup = _employeeGroupManager.Search(parameters, page, pageSize);

            if (pagedemployeeGroup is null || !pagedemployeeGroup.Items.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, pagedemployeeGroup));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var employeeGroup = _employeeGroupManager.GetById(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, employeeGroup));
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] CreateEmployeeGroupViewModel request)
        {
            var employeeGroup = _employeeGroupManager.Update(id, request, Guid.Empty);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, employeeGroup));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _employeeGroupManager.Delete(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}