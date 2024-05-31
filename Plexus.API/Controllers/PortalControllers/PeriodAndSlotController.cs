using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Client.ViewModel.Registration;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
	// TODO : Update requester from Empty Guid to actual user id

	[ApiController]
	[Route(_pathPrefix + "periods")]
	public class PeriodAndSlotController : BaseController
	{
		private readonly IPeriodAndSlotManager _periodAndSlotManager;
		private readonly ISlotConditionManager _slotConditionManager;

		public PeriodAndSlotController(IPeriodAndSlotManager periodAndSlotManager,
									   ISlotConditionManager slotConditionManager)
		{
			_periodAndSlotManager = periodAndSlotManager;
			_slotConditionManager = slotConditionManager;
		}

		[HttpPost]
		public IActionResult CreatePeriod([FromBody] CreatePeriodViewModel request)
		{
			var response = _periodAndSlotManager.CreatePeriod(request, Guid.Empty);

			return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, response));
		}

		[HttpGet("search")]
		public IActionResult SearchPeriod(int page = 1, int pageSize = 25)
		{
			var pagedResponse = _periodAndSlotManager.GetPagedPeriod(page, pageSize);

			if (!pagedResponse.Items.Any())
			{
				return NoContent();
			}

			return Ok(ResponseWrapper.Success(HttpStatusCode.OK, pagedResponse));
		}

		[HttpGet("{id}")]
		public IActionResult GetPeriodById(Guid id)
		{
			var period = _periodAndSlotManager.GetPeriodById(id);

			return Ok(ResponseWrapper.Success(HttpStatusCode.OK, period));
		}

		[HttpPut("{periodId}")]
		public IActionResult UpdatePeriod(Guid periodId, [FromBody] CreatePeriodViewModel request)
		{
			var response = _periodAndSlotManager.UpdatePeriod(periodId, request, Guid.Empty);

			return Ok(ResponseWrapper.Success(HttpStatusCode.OK, response));
		}

		[HttpDelete("{id}")]
		public IActionResult DeletePeriod(Guid id)
		{
			_periodAndSlotManager.DeletePeriod(id);

			return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
		}

		[HttpPost("{periodId}/slots")]
		public IActionResult CreateSlot(Guid periodId, [FromBody] CreateSlotViewModel request)
		{
			var slot = _periodAndSlotManager.CreateSlot(periodId, request, Guid.Empty);

			return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, slot));
		}

		[HttpGet("{periodId}/slots")]
		public IActionResult GetSlots(Guid periodId, [FromQuery] int page = 1, int pageSize = 25)
		{
			var pagedSlot = _periodAndSlotManager.GetPagedSlotByPeriodId(periodId, page, pageSize);

			if (!pagedSlot.Items.Any())
			{
				return NoContent();
			}

			return Ok(ResponseWrapper.Success(HttpStatusCode.OK, pagedSlot));
		}

		[HttpGet("slots/{slotId}")]
		public IActionResult GetSlot(Guid slotId)
		{
			var slot = _periodAndSlotManager.GetSlotById(slotId);

			return Ok(ResponseWrapper.Success(HttpStatusCode.OK, slot));
		}

		[HttpPut("{periodId}/slots/{slotId}")]
		public IActionResult UpdateSlot(Guid periodId, Guid slotId, [FromBody] CreateSlotViewModel request)
		{
			var slot = _periodAndSlotManager.UpdateSlot(periodId, slotId, request, Guid.Empty);

			return Ok(ResponseWrapper.Success(HttpStatusCode.OK, slot));
		}

		[HttpDelete("{periodId}/slots/{slotId}")]
		public IActionResult DeleteSlot(Guid periodId, Guid slotId)
		{
			_periodAndSlotManager.DeleteSlot(periodId, slotId);

			return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
		}

		[HttpPost("slots/{slotId}/conditions")]
		public IActionResult CreateSlotCondition(Guid slotId, [FromBody] CreateSlotConditionViewModel request)
		{
			var slotCondition = _slotConditionManager.Create(slotId, request, Guid.Empty);

			return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, slotCondition));
		}

		[HttpGet("slots/conditions/search")]
		public IActionResult SearchSlotConditions([FromQuery] SearchSlotConditionCriteriaViewModel parameters, int page = 1, int pageSize = 25)
		{
			var slotConditions = _slotConditionManager.Search(parameters, page, pageSize);

			if (slotConditions is null || !slotConditions.Items.Any())
			{
				return NoContent();
			}

			return Ok(ResponseWrapper.Success(HttpStatusCode.OK, slotConditions));
		}

		[HttpGet("slots/conditions/{id}")]
		public IActionResult GetSlotConditionById(Guid id)
		{
			var slotCondition = _slotConditionManager.GetById(id);

			return Ok(ResponseWrapper.Success(HttpStatusCode.OK, slotCondition));
		}

		[HttpPut("slots/{slotId}/conditions/{id}")]
		public IActionResult UpdateSlotCondition(Guid slotId, Guid id, [FromBody] CreateSlotConditionViewModel request)
		{
			var slotCondition = _slotConditionManager.Update(slotId, id, request, Guid.Empty);

			return Ok(ResponseWrapper.Success(HttpStatusCode.OK, slotCondition));
		}

		[HttpDelete("slots/conditions/{id}")]
		public IActionResult DeleteSlotCondition(Guid id)
		{
			_slotConditionManager.Delete(id);

			return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
		}
	}
}

