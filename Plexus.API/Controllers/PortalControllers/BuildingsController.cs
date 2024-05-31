using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.Facility;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Entity.DTO;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
    // TODO : UPDATE DEFAULT GUID TO USERID

    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class BuildingsController : BaseController
    {
        private readonly IBuildingManager _buildingManager;

        public BuildingsController(IBuildingManager buildingManager)
        {
            _buildingManager = buildingManager;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateBuildingViewModel request)
        {
            var building = _buildingManager.Create(request, Guid.Empty);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, building));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchBuildingCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedBuilding = _buildingManager.Search(parameters, page, pageSize);

            if (pagedBuilding is null || !pagedBuilding.Items.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, pagedBuilding));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var building = _buildingManager.GetById(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, building));
        }

        [HttpPut]
        public IActionResult Update([FromBody] BuildingViewModel request)
        {
            var building = _buildingManager.Update(request, Guid.Empty);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, building));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _buildingManager.Delete(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpGet("{id}/times")]
        public IActionResult GetAvailableTimes(Guid id)
        {
            var times = _buildingManager.GetAvailableTimes(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, times));
        }

        [HttpPut("{id}/times")]
        public IActionResult UpdateAvailableTimes(Guid id, [FromBody] IEnumerable<BuildingAvailableTimeViewModel> requests)
        {
            var times = _buildingManager.UpdateAvailableTimes(id, requests);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, times));
        }
    }
}

