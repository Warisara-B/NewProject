using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Entity.DTO;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class EmployeesController : BaseController
    {
        private readonly IEmployeeManager _employeeManager;

        public EmployeesController(IEmployeeManager employeeManager)
        {
            _employeeManager = employeeManager;
        }

        // TODO: Get user id
        [HttpPost]
        public IActionResult Create([FromBody] CreateEmployeeViewModel request)
        {
            var instructor = _employeeManager.Create(request, Guid.Empty);

            return StatusCode(201,
                ResponseWrapper.Success(HttpStatusCode.Created, instructor));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchEmployeeCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedInstructor = _employeeManager.Search(parameters, page, pageSize);

            if (pagedInstructor is null || !pagedInstructor.Items.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200,
                ResponseWrapper.Success(HttpStatusCode.OK, pagedInstructor));
        }

        [HttpGet("emails")]
        public IActionResult GetEmails([FromQuery] SearchCriteriaViewModel parameters)
        {
            var emails = _employeeManager.GetEmails(parameters);

            if (emails is null || !emails.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200,
                ResponseWrapper.Success(HttpStatusCode.OK, emails));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var instructor = _employeeManager.GetById(id);

            return StatusCode(200,
                ResponseWrapper.Success(HttpStatusCode.OK, instructor));
        }

        // TODO: Get user id
        [HttpPut]
        public IActionResult Update([FromBody] EmployeeViewModel request)
        {
            var instructor = _employeeManager.Update(request, Guid.Empty);

            return StatusCode(200,
                ResponseWrapper.Success(HttpStatusCode.OK, instructor));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _employeeManager.Delete(id);

            return StatusCode(200,
                ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpPut("{id}/cardImage")]
        public async Task<IActionResult> UploadCardImageAsync(Guid id, [FromForm] IFormFile cardImage)
        {
            await _employeeManager.UploadCardImageAsync(id, cardImage);

            return StatusCode(200,
                ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpPut("{id}/general")]
        public IActionResult UpdateEmployeeGeneralInformation(Guid id, [FromBody] UpdateEmployeeGeneralInformationViewModel request)
        {
            var employee = _employeeManager.UpdateEmployeeGeneralInformation(id, request);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, employee));
        }

        [HttpPut("{id}/work")]
        public IActionResult UpdateEmployeeWorkInformation(Guid id, [FromBody] UpdateEmployeeWorkInformationViewModel request)
        {
            var employee = _employeeManager.UpdateEmployeeWorkInformation(id, request);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, employee));
        }

        [HttpPut("{id}/educations")]
        public IActionResult UpdateEmployeeEducationalBackground(Guid id, [FromBody] IEnumerable<EmployeeEducationalBackgroundViewModel> request)
        {
            var employee = _employeeManager.UpdateEmployeeEducationalBackground(id, request);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, employee));
        }
    }
}