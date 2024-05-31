using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Client.ViewModel.Academic.Curriculum;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class AcademicSpecializationsController : BaseController
    {
        private readonly IAcademicSpecializationManager _academicSpecializationManager;

        public AcademicSpecializationsController(IAcademicSpecializationManager academicSpecializationManager)
        {
            _academicSpecializationManager = academicSpecializationManager;
        }

        // TODO: Get user id
        [HttpPost]
        public IActionResult Create([FromBody] CreateAcademicSpecializationViewModel request)
        {
            var academicSpecialization = _academicSpecializationManager.Create(request, Guid.Empty);

            return StatusCode(201,
                ResponseWrapper.Success(HttpStatusCode.Created, academicSpecialization));
        }

        [HttpGet("search")]
        public IActionResult SearchAcademicSpecialization([FromQuery] SearchAcademicSpecializationCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedAcademicSpecialization = _academicSpecializationManager.Search(parameters, page, pageSize);

            if (pagedAcademicSpecialization is null || !pagedAcademicSpecialization.Items.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, pagedAcademicSpecialization));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var academicSpecialization = _academicSpecializationManager.GetById(id);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, academicSpecialization));
        }

        // TODO: Get user id
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] CreateAcademicSpecializationViewModel request)
        {
            var academicSpecialization = _academicSpecializationManager.Update(id, request, Guid.Empty);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, academicSpecialization));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _academicSpecializationManager.Delete(id);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpGet("{id}/courses")]
        public IActionResult GetSpecializationCourses(Guid id)
        {
            var courses = _academicSpecializationManager.GetCourses(id)
                                                        .ToList();

            if (!courses.Any())
            {
                return NoContent();
            }

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, courses));
        }

        [HttpPut("{id}/courses")]
        public IActionResult UpdateSpecializationCourses(Guid id, IEnumerable<CreateSpecializationCourseViewModel> requests)
        {
            var courses = _academicSpecializationManager.UpdateCourses(id, requests)
                                                        .ToList();

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, courses));
        }
    }
}