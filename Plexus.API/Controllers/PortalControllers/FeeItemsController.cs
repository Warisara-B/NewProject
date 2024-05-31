using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.Payment;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Entity.DTO;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class FeeItemsController : BaseController
    {
        private readonly IFeeItemManager _feeItemManager;

        public FeeItemsController(IFeeItemManager feeItemManager)
        {
            _feeItemManager = feeItemManager;
        }

        // TODO: Get user id
        [HttpPost]
        public IActionResult Create(CreateFeeItemViewModel request)
        {
            var feeItem = _feeItemManager.Create(request, Guid.Empty);

            return StatusCode(201,
                ResponseWrapper.Success(HttpStatusCode.Created, feeItem));
        }

        [HttpGet("search")]
        public IActionResult SearchFeeTypes([FromQuery] SearchFeeItemCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedFeeItem = _feeItemManager.Search(parameters, page, pageSize);

            if (pagedFeeItem is null || !pagedFeeItem.Items.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200,
                ResponseWrapper.Success(HttpStatusCode.OK, pagedFeeItem));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var feeItem = _feeItemManager.GetById(id);

            return StatusCode(200,
                ResponseWrapper.Success(HttpStatusCode.OK, feeItem));
        }

        // TODO: Get user id
        [HttpPut]
        public IActionResult Update(Guid id, CreateFeeItemViewModel request)
        {
            var feeItem = _feeItemManager.Update(id, request, Guid.Empty);

            return StatusCode(200,
                ResponseWrapper.Success(HttpStatusCode.OK, feeItem));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _feeItemManager.Delete(id);

            return StatusCode(200,
                ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}