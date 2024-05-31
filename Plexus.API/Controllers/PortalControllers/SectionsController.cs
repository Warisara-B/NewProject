using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.Academic.Section;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class SectionsController : BaseController
    {
        private readonly ISectionManager _sectionManager;
        private readonly IStudyCourseManager _studyCourseManager;
        private readonly IExclusionConditionManager _exclusionConditionManager;

        public SectionsController(ISectionManager sectionManager,
                                  IStudyCourseManager studyCourseManager,
                                  IExclusionConditionManager exclusionConditionManager)
        {
            _sectionManager = sectionManager;
            _studyCourseManager = studyCourseManager;
            _exclusionConditionManager = exclusionConditionManager;
        }

        [HttpPost]
        public IActionResult Create(CreateSectionViewModel request)
        {
            var section = _sectionManager.Create(request);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, section));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchSectionCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedSection = _sectionManager.Search(parameters, page, pageSize);

            if (pagedSection is null || !pagedSection.Items.Any())
            {
                return StatusCode(204);
            }

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, pagedSection));
        }

        [HttpPut("{id}/status")]
        public IActionResult UpdateStatus(Guid id)
        {
            _sectionManager.UpdateStatus(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _sectionManager.Delete(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpGet("{id}/studyCourses")]
        public IActionResult GetStudyCourses(Guid id)
        {
            var studyCourses = _studyCourseManager.GetBySectionId(id);

            if (studyCourses is null || !studyCourses.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, studyCourses));
        }

        [HttpPut("{id}/studyCourses")]
        public IActionResult UpdateStudyCourses(Guid id, IEnumerable<UpdateStudyCourseViewModel> requests)
        {
            var studyCourses = _studyCourseManager.Update(requests, Guid.Empty, sectionId: id);

            return StatusCode(200,
                ResponseWrapper.Success(HttpStatusCode.OK, studyCourses));
        }

        [HttpPost("{id}/exclusionConditions")]
        public IActionResult CreateExclusionCondition(Guid id, [FromBody] CreateExclusionConditionViewModel request)
        {
            var exclusionCondition = _exclusionConditionManager.Create(id, request, Guid.Empty);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, exclusionCondition));
        }

        [HttpGet("{id}/exclusionConditions")]
        public IActionResult GetExclusionConditionBySectionId(Guid id)
        {
            var exclusionConditions = _exclusionConditionManager.GetBySectionId(id);

            if (exclusionConditions is null || !exclusionConditions.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, exclusionConditions));
        }

        [HttpGet("exclusionConditions/{id}")]
        public IActionResult GetExclusionConditionById(Guid id)
        {
            var exclusionCondition = _exclusionConditionManager.GetById(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, exclusionCondition));
        }

        [HttpPut("exclusionConditions")]
        public IActionResult UpdateExclusionCondition([FromBody] ExclusionConditionViewModel request)
        {
            var exclusionCondition = _exclusionConditionManager.Update(request, Guid.Empty);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, exclusionCondition));
        }

        [HttpDelete("exclusionConditions/{id}")]
        public IActionResult DeleteExclusionCondition(Guid id)
        {
            _exclusionConditionManager.Delete(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}