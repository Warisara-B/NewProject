using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.Facility;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
    // TODO : UPDATE DEFAULT GUID TO USERID

    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class RoomTypesController : BaseController
    {
        private readonly IRoomTypeManager _roomTypeManager;

        public RoomTypesController(IRoomTypeManager roomTypeManager)
        {
            _roomTypeManager = roomTypeManager;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateRoomTypeViewModel request)
        {
            var room = _roomTypeManager.Create(request, Guid.Empty);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, room));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchRoomTypeCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedRoom = _roomTypeManager.Search(parameters, page, pageSize);

            if (pagedRoom is null || !pagedRoom.Items.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, pagedRoom));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var room = _roomTypeManager.GetById(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, room));
        }

        [HttpPut]
        public IActionResult Update([FromBody] RoomTypeViewModel request)
        {
            var room = _roomTypeManager.Update(request, Guid.Empty);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, room));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _roomTypeManager.Delete(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}

