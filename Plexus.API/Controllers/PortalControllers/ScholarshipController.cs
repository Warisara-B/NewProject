using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.Payment.Scholarship;
using Plexus.Entity.DTO;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class ScholarshipController : BaseController
    {
        private readonly IScholarshipTypeManager _scholarshipTypeManager;
        private readonly IScholarshipManager _scholarshipManager;
        private readonly IStudentScholarshipManager _studentScholarshipManager;

        public ScholarshipController(IScholarshipTypeManager scholarshipTypeManager,
                                     IScholarshipManager scholarshipManager,
                                     IStudentScholarshipManager studentScholarshipManager)
        {
            _scholarshipTypeManager = scholarshipTypeManager;
            _scholarshipManager = scholarshipManager;
            _studentScholarshipManager = studentScholarshipManager;
        }

        [HttpPost]
        public IActionResult CreateScholarship([FromBody] CreateScholarshipViewModel request)
        {
            var scholarship = _scholarshipManager.Create(request, Guid.Empty);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, scholarship));
        }

        [HttpGet("search")]
        public IActionResult SearchScholarship([FromQuery] SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedScholarship = _scholarshipManager.Search(parameters, page, pageSize);

            if (pagedScholarship is null || !pagedScholarship.Items.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, pagedScholarship));
        }

        [HttpGet("{id}")]
        public IActionResult GetScholarshipById(Guid id)
        {
            var scholarship = _scholarshipManager.GetById(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, scholarship));
        }

        [HttpPut]
        public IActionResult UpdateScholarship([FromBody] ScholarshipViewModel request)
        {
            var scholarship = _scholarshipManager.Update(request, Guid.Empty);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, scholarship));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteScholarship(Guid id)
        {
            _scholarshipManager.Delete(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpPost("types")]
        public IActionResult CreateScholarshipType([FromBody] CreateScholarshipTypeViewModel request)
        {
            var scholarshipType = _scholarshipTypeManager.Create(request, Guid.Empty);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, scholarshipType));
        }

        [HttpGet("types/search")]
        public IActionResult SearchScholarshipTypes([FromQuery] SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedScholarshipType = _scholarshipTypeManager.Search(parameters, page, pageSize);

            if (pagedScholarshipType is null || !pagedScholarshipType.Items.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, pagedScholarshipType));
        }

        [HttpGet("types/{id}")]
        public IActionResult GetScholarshipTypeById(Guid id)
        {
            var scholarshipType = _scholarshipTypeManager.GetById(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, scholarshipType));
        }

        [HttpPut("types")]
        public IActionResult UpdateScholarshipType([FromBody] ScholarshipTypeViewModel request)
        {
            var scholarshipType = _scholarshipTypeManager.Update(request, Guid.Empty);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, scholarshipType));
        }

        [HttpDelete("types/{id}")]
        public IActionResult DeleteScholarshipType(Guid id)
        {
            _scholarshipTypeManager.Delete(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpGet("{id}/reserveBudgets")]
        public IActionResult GetReserveBudgetByScholarshipId(Guid id)
        {
            var budgets = _scholarshipManager.GetReserveBudgetByScholarshipId(id);

            if (budgets is null || !budgets.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, budgets));
        }

        [HttpPut("{id}/reserveBudgets")]
        public IActionResult UpdateReserveBudgets(Guid id, [FromBody] IEnumerable<ScholarshipReserveBudgetViewModel> budgets)
        {
            var reserveBudgets = _scholarshipManager.UpdateReserveBudgets(id, budgets);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, reserveBudgets));
        }

        [HttpPost("{id}/students")]
        public IActionResult CreateStudentScholarships(Guid id, [FromBody] CreateMultipleScholarshipViewModel request)
        {
            var studentScholarships = _studentScholarshipManager.Create(id, request, Guid.Empty);

            return StatusCode(200,
                    ResponseWrapper.Success(HttpStatusCode.OK, studentScholarships));
        }

        [HttpGet("students/search")]
        public IActionResult SearchStudentScholarships([FromQuery] SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedStudentScholarship = _studentScholarshipManager.Search(parameters, page, pageSize);

            if (pagedStudentScholarship is null || !pagedStudentScholarship.Items.Any())
            {
				return StatusCode(204);
            }

			return StatusCode(200,
				ResponseWrapper.Success(HttpStatusCode.OK, pagedStudentScholarship));
        }

        [HttpGet("students/{id}")]
        public IActionResult GetStudentScholarships(Guid id)
        {
            var studentScholarship = _studentScholarshipManager.GetById(id);

            return StatusCode(200,
                    ResponseWrapper.Success(HttpStatusCode.OK, studentScholarship));
        }

        [HttpPut("students")]
        public IActionResult UpdateStudentScholarships([FromBody] UpdateStudentScholarShipViewModel request)
        {
            var studentScholarship = _studentScholarshipManager.Update(request, Guid.Empty);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, studentScholarship));
        }

        [HttpPut("students/{id}/active")]
        public IActionResult ActiveStudentScholarships(Guid id, bool isActive)
        {
            _studentScholarshipManager.Active(id, isActive, Guid.Empty);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpPut("students/{id}/approve")]
        public IActionResult ApproveStudentScholarships(Guid id, [FromBody] ApproveStudentScholarshipViewModel request)
        {
            _studentScholarshipManager.Approve(id, request, Guid.Empty);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpDelete("students/{id}")]
        public IActionResult DeleteStudentScholarships(Guid id)
        {
            _studentScholarshipManager.Delete(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpPost("students/{id}/reserveBudgets")]
        public IActionResult AddStudentScholarshipReserveBudget(Guid id, [FromBody] CreateStudentReservedBudgetViewModel request)
        {
            _studentScholarshipManager.CreateNewReserveBudget(id, request, Guid.Empty);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created));
        }

        [HttpGet("students/reserveBudgets/{id}")]
        public IActionResult GetBudgetById(Guid id)
        {
            var budget = _studentScholarshipManager.GetBudgetById(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, budget));
        }

        [HttpPut("students/{id}/reserveBudgets")]
        public IActionResult UpdateStudentScholarshipReserveBudget(Guid id, [FromBody] UpdateStudentReservedBudgetViewModel request)
        {
            _studentScholarshipManager.UpdateReserveBudgetBalance(id, request, Guid.Empty);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpPost("students/{id}/reserveBudgets/adjust")]
        public IActionResult InsertAdjustBalance(Guid id, [FromBody] CreateStudentReservedBudgetViewModel request)
        {
            _studentScholarshipManager.AddAdjustmentBalance(id, request, Guid.Empty);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpPost("students/{id}/reserveBudgets/use")]
        public IActionResult UseBudget(Guid id, [FromBody] UpdateStudentReservedBudgetViewModel request)
        {
            _studentScholarshipManager.UseReserveBudget(id, request, Guid.Empty);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpGet("{id}/feeItems")]
        public IActionResult GetFeeItemByScholarshipId(Guid id)
        {
            var feeItems = _scholarshipManager.GetFeeItemByScholarshipId(id);

            if (feeItems is null || !feeItems.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, feeItems));
        }

        [HttpPut("{id}/feeItems")]
        public IActionResult UpdateFeeItems(Guid id, [FromBody] IEnumerable<UpdateScholarshipFeeItemViewModel> items)
        {
            var feeItems = _scholarshipManager.UpdateFeeItems(id, items, Guid.Empty);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, feeItems));
        }
    }
}
