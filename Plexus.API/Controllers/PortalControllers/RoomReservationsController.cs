using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.Facility.Reservation;
using Plexus.Database.Enum.Facility.Reservation;
using Plexus.Entity.DTO;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
    [ApiController]
	[Route(_pathPrefix + "[controller]")]
    public class RoomReservationsController : BaseController
    {
        private readonly IRoomReservationManager _roomReservationManager;

        public RoomReservationsController(IRoomReservationManager roomReservationManager)
        {
            _roomReservationManager = roomReservationManager;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateRoomReserveRequestViewModel request)
        {
            var reserveRequest = _roomReservationManager.Create(request, Guid.Empty);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, reserveRequest));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedReserveRequest = _roomReservationManager.Search(parameters, page, pageSize);

            if (pagedReserveRequest is null || !pagedReserveRequest.Items.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, pagedReserveRequest));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var reserveRequest = _roomReservationManager.GetById(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, reserveRequest));
        }

        [HttpGet("{id}/slots")]
        public IActionResult GetReserveSlotByRequestId(Guid id)
        {
            var reserveSlots = _roomReservationManager.GetReserveSlotByRequestId(id);

            if (reserveSlots is null || !reserveSlots.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, reserveSlots));
        }

        [HttpPut("{id}/status")]
        public IActionResult UpdateStatus(Guid id, [FromQuery] ReservationStatus status, [FromQuery] string? remark)
        {
            _roomReservationManager.UpdateStatus(id, status, remark, Guid.Empty);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpPut("slots/cancel")]
        public IActionResult CancelSlotById([FromBody] IEnumerable<UpdateRoomReserveSlotViewModel> requests)
        {
            _roomReservationManager.CancelReserveSlots(requests, Guid.Empty);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}