using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.Academic;
using Plexus.Entity.DTO;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
	[ApiController]
	[Route(_pathPrefix + "[controller]")]
	public class GradesController : BaseController
	{
		private readonly IGradeManager _gradeManager;

		public GradesController(IGradeManager gradeManager)
		{
			_gradeManager = gradeManager;
		}

        [HttpPost]
		public IActionResult Create(CreateGradeViewModel request)
        {
			var grade = _gradeManager.Create(request, Guid.Empty);

			return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, grade));
        }

		[HttpGet("search")]
		public IActionResult Search([FromQuery] SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
		{
			var pagedGrades = _gradeManager.Search(parameters, page, pageSize);

			if (pagedGrades is null || !pagedGrades.Items.Any())
            {
				return StatusCode(204);
            }

			return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, pagedGrades));
		}

		[HttpGet("{id}")]
		public IActionResult GetById(Guid id)
		{
			var grade = _gradeManager.GetById(id);

			return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, grade));
		}

        [HttpPut]
		public IActionResult Update(GradeViewModel request)
        {
			var grade = _gradeManager.Update(request, Guid.Empty);

			return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, grade));
        }

        [HttpDelete("{id}")]
		public IActionResult Delete(Guid id)
		{
			_gradeManager.Delete(id);

			return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
		}
	}
}

