using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.Payment;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Entity.DTO;
using Plexus.Utility;
using System.Net;

namespace Plexus.API.Controllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class FeeGroupsController : BaseController
    {
        private readonly IFeeGroupManager _feeGroupManager;

        public FeeGroupsController(IFeeGroupManager feeGroupManager)
        {
            _feeGroupManager = feeGroupManager;
        }

        // TODO: Get user id
        [HttpPost]
        public IActionResult Create(CreateFeeGroupViewModel request)
        {
            var feeGroup = _feeGroupManager.Create(request, Guid.Empty);

            return StatusCode(201,
                ResponseWrapper.Success(HttpStatusCode.Created, feeGroup));
        }

        [HttpGet("search")]
        public IActionResult SearchFeeGroups([FromQuery] SearchFeeGroupCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedFeeGroup = _feeGroupManager.Search(parameters, page, pageSize);

            if (pagedFeeGroup is null || !pagedFeeGroup.Items.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200,
                ResponseWrapper.Success(HttpStatusCode.OK, pagedFeeGroup));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var feeGroup = _feeGroupManager.GetById(id);

            return StatusCode(200,
                ResponseWrapper.Success(HttpStatusCode.OK, feeGroup));
        }

        // TODO: Get user id
        [HttpPut]
        public IActionResult Update(Guid id, CreateFeeGroupViewModel request)
        {
            var feeGroup = _feeGroupManager.Update(id, request, Guid.Empty);

            return StatusCode(200,
                ResponseWrapper.Success(HttpStatusCode.OK, feeGroup));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _feeGroupManager.Delete(id);

            return StatusCode(200,
                ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}