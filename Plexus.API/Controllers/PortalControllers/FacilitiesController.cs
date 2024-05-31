using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.Facility;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Entity.DTO;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class FacilitiesController : BaseController
    {
        private readonly IFacilityManager _facilityManager;

        public FacilitiesController(IFacilityManager facilityManager)
        {
            _facilityManager = facilityManager;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateFacilityViewModel request)
        {
            var facility = _facilityManager.Create(request, Guid.Empty);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, facility));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchFacilityCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedFacility = _facilityManager.Search(parameters, page, pageSize);

            if (pagedFacility is null || !pagedFacility.Items.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, pagedFacility));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var facility = _facilityManager.GetById(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, facility));
        }

        [HttpPut]
        public IActionResult Update(Guid id, [FromBody] CreateFacilityViewModel request)
        {
            var facility = _facilityManager.Update(id, request, Guid.Empty);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, facility));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _facilityManager.Delete(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}