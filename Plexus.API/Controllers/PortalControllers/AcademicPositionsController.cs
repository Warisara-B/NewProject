using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Utility;

namespace Plexus.API.Controllers.PortalControllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class AcademicPositionsController : BaseController
    {
        private readonly IAcademicPositionManager _academicPositionManager;

        public AcademicPositionsController(IAcademicPositionManager academicPositionManager)
        {
            _academicPositionManager = academicPositionManager;
        }

        [HttpPost]
        public IActionResult Create(CreateAcademicPositionViewModel request)
        {
            var academicPosition = _academicPositionManager.Create(request);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, academicPosition));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchAcademicPositionCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedAcademicPosition = _academicPositionManager.Search(parameters, page, pageSize);

            if (pagedAcademicPosition is null || !pagedAcademicPosition.Items.Any())
            {
                return NoContent();
            }

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, pagedAcademicPosition));
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, CreateAcademicPositionViewModel request)
        {
            var academicPosition = _academicPositionManager.Update(id, request);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, academicPosition));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _academicPositionManager.Delete(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}