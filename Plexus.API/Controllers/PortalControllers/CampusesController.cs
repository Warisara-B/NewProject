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
    public class CampusesController : BaseController
    {
        private readonly ICampusManager _campusManager;

        public CampusesController(ICampusManager campusManager)
        {
            _campusManager = campusManager;
        }

        // TODO: Get user id
        [HttpPost]
        public IActionResult Create(CreateCampusViewModel request)
        {
            var campus = _campusManager.Create(request, Guid.Empty);

            return StatusCode(201,
                ResponseWrapper.Success(HttpStatusCode.Created, campus));
        }

        [HttpGet("search")]
        public IActionResult SearchCampus([FromQuery] SearchCampusCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedCampus = _campusManager.Search(parameters, page, pageSize);

            if (pagedCampus is null || !pagedCampus.Items.Any())
            {
                return NoContent();
            }

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, pagedCampus));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var campus = _campusManager.GetById(id);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, campus));
        }

        // TODO: Get user id
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, CreateCampusViewModel request)
        {
            var campus = _campusManager.Update(id, request, Guid.Empty);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK, campus));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _campusManager.Delete(id);

            return Ok(
                ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}