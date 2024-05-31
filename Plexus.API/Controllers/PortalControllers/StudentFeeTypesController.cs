using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.Payment;
using Plexus.Entity.DTO;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
    [ApiController]
	[Route(_pathPrefix + "[controller]")]
    public class StudentFeeTypesController : BaseController
    {
        private readonly IStudentFeeTypeManager _studentFeeTypeManager;

        public StudentFeeTypesController(IStudentFeeTypeManager studentFeeTypeManager)
        {
            _studentFeeTypeManager = studentFeeTypeManager;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateStudentFeeTypeViewModel request)
        {
            var studentFeeType = _studentFeeTypeManager.Create(request, Guid.Empty);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, studentFeeType));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedStudentFeeType = _studentFeeTypeManager.Search(parameters, page, pageSize);

            if (pagedStudentFeeType is null || !pagedStudentFeeType.Items.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, pagedStudentFeeType));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var studentFeeType = _studentFeeTypeManager.GetById(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, studentFeeType));
        }

        [HttpPut]
		public IActionResult Update([FromBody] StudentFeeTypeViewModel request)
        {
			var studentFeeType = _studentFeeTypeManager.Update(request, Guid.Empty);

			return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, studentFeeType));
        }

        [HttpDelete("{id}")]
		public IActionResult Delete(Guid id)
		{
			_studentFeeTypeManager.Delete(id);

			return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
		}
    }
}