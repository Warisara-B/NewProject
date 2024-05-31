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
    public class CareerPositionsController : BaseController
    {
        private readonly ICareerPositionManager _careerPositionManager;

        public CareerPositionsController(ICareerPositionManager careerPositionManager)
        {
            _careerPositionManager = careerPositionManager;
        }

        [HttpPost]
        public IActionResult Create(CreateCareerPositionViewModel request)
        {
            var careerPosition = _careerPositionManager.Create(request);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, careerPosition));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchCareerPositionCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedCareerPosition = _careerPositionManager.Search(parameters, page, pageSize);

            if (pagedCareerPosition is null || !pagedCareerPosition.Items.Any())
            {
                return NoContent();
            }

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, pagedCareerPosition));
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, CreateCareerPositionViewModel request)
        {
            var careerPosition = _careerPositionManager.Update(id, request);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, careerPosition));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _careerPositionManager.Delete(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}