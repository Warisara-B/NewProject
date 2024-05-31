using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.Payment;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class CourseRatesController : BaseController
    {
        private readonly ICourseRateManager _courseRateManager;

        public CourseRatesController(ICourseRateManager courseRateManager)
        {
            _courseRateManager = courseRateManager;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateCourseRateViewModel request)
        {
            var courseRate = _courseRateManager.Create(request);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, courseRate));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var courseRate = _courseRateManager.GetById(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, courseRate));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchCourseRateCriteriaViewModel parameters, int page, int pageSize)
        {
            var courseRates = _courseRateManager.Search(parameters, page, pageSize);

            if (courseRates.Items is null || !courseRates.Items.Any())
            {
                return NoContent();
            }

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, courseRates));
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] CreateCourseRateViewModel request)
        {
            var courseRate = _courseRateManager.Update(id, request);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, courseRate));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _courseRateManager.Delete(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}

