using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel;
using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Database.Enum.Academic;
using Plexus.Entity.DTO;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
	[ApiController]
	[Route(_pathPrefix + "[controller]")]
	public class TermsController : BaseController
	{
		private readonly ITermManager _termManagement;

		public TermsController(ITermManager termManagement)
		{
			_termManagement = termManagement;
		}

		// TODO: Get user id
		[HttpPost]
		public IActionResult Create(CreateTermViewModel request)
		{
			var term = _termManagement.Create(request, Guid.Empty);

			return StatusCode(201,
				ResponseWrapper.Success(HttpStatusCode.Created, term));
		}

		[HttpGet("search")]
		public IActionResult SearchTerm([FromQuery] SearchTermCriteriaViewModel parameters, int page = 1, int pageSize = 25)
		{
			var pagedTerm = _termManagement.Search(parameters, page, pageSize);

			if (pagedTerm is null || !pagedTerm.Items.Any())
			{
				return NoContent();
			}

			return Ok(
				ResponseWrapper.Success(HttpStatusCode.OK, pagedTerm));
		}

		[HttpGet("{id}")]
		public IActionResult GetById(Guid id)
		{
			var term = _termManagement.GetById(id);

			return Ok(
				ResponseWrapper.Success(HttpStatusCode.OK, term));
		}

		[HttpPost("check/conflict")]
		public IActionResult CheckTermStatusConflict([FromBody] TermStatusCheckViewModel criteria)
		{
			var term = _termManagement.CheckStatus(criteria);

			return Ok(
				ResponseWrapper.Success(HttpStatusCode.OK, term));
		}

		// TODO: Get user id
		[HttpPut("{id}")]
		public IActionResult Update(Guid id, CreateTermViewModel request)
		{
			var term = _termManagement.Update(id, request, Guid.Empty);

			return Ok(
				ResponseWrapper.Success(HttpStatusCode.OK, term));
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(Guid id)
		{
			_termManagement.Delete(id);

			return Ok(
				ResponseWrapper.Success(HttpStatusCode.OK));
		}
	}
}

