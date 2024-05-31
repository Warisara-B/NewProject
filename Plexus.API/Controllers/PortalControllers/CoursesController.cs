using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.Payment;
using Plexus.Client.ViewModel.Registration;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
	// TODO : UPDATE EMPTY GUID TO ACTUAL USER ID

	[ApiController]
	[Route(_pathPrefix + "[controller]")]
	public class CoursesController : BaseController
	{
		private readonly ICourseManager _courseManager;
		private readonly ICourseTopicManager _courseTopicManager;
		private readonly ICourseFeeManager _courseFeeManager;
		private readonly IPrerequisiteManager _prerequisiteManager;

		public CoursesController(ICourseManager courseManager,
								 ICourseTopicManager courseTopicManager,
								 ICourseFeeManager courseFeeManager,
								 IPrerequisiteManager prerequisiteManager)
		{
			_courseManager = courseManager;
			_courseTopicManager = courseTopicManager;
			_courseFeeManager = courseFeeManager;
			_prerequisiteManager = prerequisiteManager;
		}

		[HttpPost]
		public IActionResult Create(CreateCourseViewModel request)
		{
			var course = _courseManager.Create(request, Guid.Empty);

			return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, course));
		}

		[HttpGet("search")]
		public IActionResult Search([FromQuery] SearchCourseCriteriaViewModel parameters, int page, int pageSize)
		{
			var courses = _courseManager.Search(parameters, page, pageSize);

			if (courses is null || !courses.Items.Any())
			{
				return NoContent();
			}

			return Ok(ResponseWrapper.Success(HttpStatusCode.OK, courses));
		}

		[HttpGet("{id}")]
		public IActionResult GetById(Guid id)
		{
			var course = _courseManager.GetById(id);

			return Ok(ResponseWrapper.Success(HttpStatusCode.OK, course));
		}

		[HttpPut("{id}")]
		public IActionResult Update(Guid id, CreateCourseViewModel request)
		{
			var course = _courseManager.Update(id, request, Guid.Empty);

			return Ok(ResponseWrapper.Success(HttpStatusCode.OK, course));
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(Guid id)
		{
			_courseManager.Delete(id);

			return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
		}

		[HttpPost("{id}/fees")]
		public IActionResult CreateCourseFee(Guid id, [FromBody] CreateCourseFeeViewModel request)
		{
			var courseFee = _courseFeeManager.Create(id, request, Guid.Empty);

			return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, courseFee));
		}

		[HttpGet("{id}/fees")]
		public IActionResult GetCourseFeeByCourseId(Guid id)
		{
			var courseFees = _courseFeeManager.GetByCourseId(id);

			if (courseFees is null || !courseFees.Any())
			{
				return NoContent();
			}

			return Ok(ResponseWrapper.Success(HttpStatusCode.OK, courseFees));
		}

		[HttpGet("fees/{id}")]
		public IActionResult GetCourseFeeById(Guid id)
		{
			var courseFee = _courseFeeManager.GetById(id);

			return Ok(ResponseWrapper.Success(HttpStatusCode.OK, courseFee));
		}

		[HttpPut("{id}/fees/{courseFeeId}")]
		public IActionResult UpdateCourseFee(Guid id, Guid courseFeeId, [FromBody] CreateCourseFeeViewModel request)
		{
			var courseFee = _courseFeeManager.Update(id, courseFeeId, request, Guid.Empty);

			return Ok(ResponseWrapper.Success(HttpStatusCode.OK, courseFee));
		}

		[HttpDelete("fees/{id}")]
		public IActionResult DeleteCourseFee(Guid id)
		{
			_courseFeeManager.Delete(id);

			return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
		}

		[HttpPost("{courseId}/topics")]
		public IActionResult CreateCourseTopic(Guid courseId, [FromBody] CreateCourseTopicViewModel request)
		{
			var courseTopic = _courseTopicManager.Create(courseId, request, Guid.Empty);

			return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, courseTopic));
		}

		[HttpGet("{courseId}/topics")]
		public IActionResult GetCourseTopicByCourseId(Guid courseId)
		{
			var courseTopics = _courseTopicManager.GetByCourseId(courseId);

			if (courseTopics is null || !courseTopics.Any())
			{
				return NoContent();
			}

			return Ok(ResponseWrapper.Success(HttpStatusCode.OK, courseTopics));
		}

		[HttpGet("topics/{id}")]
		public IActionResult GetCourseTopicById(Guid id)
		{
			var courseFee = _courseTopicManager.GetById(id);

			return Ok(ResponseWrapper.Success(HttpStatusCode.OK, courseFee));
		}

		[HttpGet("{courseId}/topics/search")]
		public IActionResult Search(Guid courseId, int page = 1, int pageSize = 25)
		{
			var courseTopics = _courseTopicManager.Search(courseId, page, pageSize);

			if (courseTopics is null || !courseTopics.Items.Any())
			{
				return NoContent();
			}

			return Ok(ResponseWrapper.Success(HttpStatusCode.OK, courseTopics));
		}


		[HttpPut("{courseId}/topics/{courseTopicId}")]
		public IActionResult UpdateCourseTopic(Guid courseId, Guid courseTopicId, [FromBody] CreateCourseTopicViewModel request)
		{
			var courseFee = _courseTopicManager.Update(courseId, courseTopicId, request, Guid.Empty);

			return Ok(ResponseWrapper.Success(HttpStatusCode.OK, courseFee));
		}

		[HttpDelete("topics/{id}")]
		public IActionResult DeleteCourseTopic(Guid id)
		{
			_courseFeeManager.Delete(id);

			return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
		}

		[HttpPut("{id}/prerequisites")]
		public IActionResult UpdatePrerequisite(Guid id, [FromBody] CreatePrerequisiteViewModel request)
		{
			var prerequisite = _prerequisiteManager.UpdateCoursePrerequisite(id, request, Guid.Empty);

			return Ok(ResponseWrapper.Success(HttpStatusCode.OK, prerequisite));
		}
	}
}

