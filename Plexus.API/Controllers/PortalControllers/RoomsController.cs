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
    public class RoomsController : BaseController
    {
        private readonly IRoomManager _roomManager;

        public RoomsController(IRoomManager roomManager)
        {
            _roomManager = roomManager;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateRoomViewModel request)
        {
            var room = _roomManager.Create(request, Guid.Empty);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, room));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchRoomCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedRoom = _roomManager.Search(parameters, page, pageSize);

            if (pagedRoom is null || !pagedRoom.Items.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, pagedRoom));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var room = _roomManager.GetById(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, room));
        }

        [HttpPut]
        public IActionResult Update([FromBody] RoomViewModel request)
        {
            var room = _roomManager.Update(request, Guid.Empty);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, room));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _roomManager.Delete(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpGet("{id}/facilities")]
        public IActionResult GetFacilityByRoomId(Guid id)
        {
            var facilities = _roomManager.GetFacilityByRoomId(id);

            if (!facilities.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, facilities));
        }
    }
}

