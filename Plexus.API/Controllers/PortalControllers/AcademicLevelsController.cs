using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Client.ViewModel.Academic;
using Plexus.Entity.DTO;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class AcademicLevelsController : BaseController
    {
        private readonly IAcademicLevelManager _academicLevelManager;

        public AcademicLevelsController(IAcademicLevelManager academicLevelManager)
        {
            _academicLevelManager = academicLevelManager;
        }

        // TODO: Get user id
        [HttpPost]
        public IActionResult Create(CreateAcademicLevelViewModel request)
        {
            var academicLevel = _academicLevelManager.Create(request, Guid.Empty);

            return StatusCode(201,
                ResponseWrapper.Success(HttpStatusCode.Created, academicLevel));
        }

        [HttpGet("search")]
        public IActionResult SearchAcademicLevel([FromQuery] SearchAcademicLevelCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedAcademicLevel = _academicLevelManager.Search(parameters, page, pageSize);

            if (pagedAcademicLevel is null || !pagedAcademicLevel.Items.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, pagedAcademicLevel));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var academicLevel = _academicLevelManager.GetById(id);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, academicLevel));
        }

        // TODO: Get user id
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, CreateAcademicLevelViewModel request)
        {
            var academicLevel = _academicLevelManager.Update(id, request, Guid.Empty);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, academicLevel));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _academicLevelManager.Delete(id);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}