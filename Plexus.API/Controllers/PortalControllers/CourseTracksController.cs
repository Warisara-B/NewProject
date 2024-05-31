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
    public class CourseTracksController : BaseController
    {
        private readonly ICourseTrackManager _courseTrackManager;

        public CourseTracksController(ICourseTrackManager courseTrackManager)
        {
            _courseTrackManager = courseTrackManager;
        }
        
        [HttpPost]
        public IActionResult Create([FromBody] CreateCourseTrackViewModel request)
        {
            var courseTrack = _courseTrackManager.Create(request, Guid.Empty);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, courseTrack));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedCourseTrack = _courseTrackManager.Search(parameters, page, pageSize);

            if (pagedCourseTrack is null || !pagedCourseTrack.Items.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, pagedCourseTrack));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var courseTrack = _courseTrackManager.GetById(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, courseTrack));
        }

        [HttpPut]
		public IActionResult Update([FromBody] CourseTrackViewModel request)
        {
			var courseTrack = _courseTrackManager.Update(request, Guid.Empty);

			return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, courseTrack));
        }

        [HttpDelete("{id}")]
		public IActionResult Delete(Guid id)
		{
			_courseTrackManager.Delete(id);

			return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
		}

        [HttpGet("{id}/details")]
        public IActionResult GetDetailByCourseTrackId(Guid id)
        {
            var courseTrackDetails = _courseTrackManager.GetDetailByCourseTrackId(id);

            if (courseTrackDetails is null || !courseTrackDetails.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, courseTrackDetails));
        }

        [HttpPut("{id}/details")]
		public IActionResult UpdateDetails(Guid id, [FromBody] IEnumerable<UpdateCourseTrackDetailViewModel> details)
        {
			var courseTrackDetails = _courseTrackManager.UpdateDetails(id, details);

			return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, courseTrackDetails));
        }
    }    
}