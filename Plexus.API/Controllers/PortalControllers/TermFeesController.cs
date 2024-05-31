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
    public class TermFeesController : BaseController
    {
        private readonly ITermFeePackageManager _termFeePackageManager;
        private readonly ITermFeeItemManager _termFeeItemManager;

        public TermFeesController(ITermFeePackageManager termFeePackageManager,
                                  ITermFeeItemManager termFeeItemManager)
        {
            _termFeePackageManager = termFeePackageManager;
            _termFeeItemManager = termFeeItemManager;
        }

        // TODO: Get user id
        [HttpPost]
        public IActionResult Create([FromBody] CreateTermFeePackageViewModel request)
        {
            var package = _termFeePackageManager.Create(request);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, package));
        }

        [HttpGet("search")]
        public IActionResult SearchTermFeePackage([FromQuery] SearchTermFeePackageCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var packages = _termFeePackageManager.Search(parameters, page, pageSize);

            if (packages is null || !packages.Items.Any())
            {
                return NoContent();
            }

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, packages));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var package = _termFeePackageManager.GetById(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, package));
        }

        // TODO: Get user id
        [HttpPut]
        public IActionResult Update(Guid id, CreateTermFeePackageViewModel request)
        {
            var package = _termFeePackageManager.Update(id, request);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, package));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _termFeePackageManager.Delete(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
        }

        // TODO: Get user id
        [HttpPost("items")]
        public IActionResult CreateItem([FromBody] CreateTermFeeItemViewModel request)
        {
            var item = _termFeeItemManager.Create(request, Guid.Empty);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, item));
        }

        [HttpGet("items/search")]
        public IActionResult SearchTermFeeItem([FromQuery] SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var items = _termFeeItemManager.Search(parameters, page, pageSize);

            if (items is null || !items.Items.Any())
            {
                return NoContent();
            }

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, items));
        }

        [HttpGet("items/{id}")]
        public IActionResult GetItemById(Guid id)
        {
            var item = _termFeeItemManager.GetById(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, item));
        }

        // TODO: Get user id
        [HttpPut("items")]
        public IActionResult UpdateItem(TermFeeItemViewModel request)
        {
            var items = _termFeeItemManager.Update(request, Guid.Empty);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK, items));
        }

        [HttpDelete("items/{id}")]
        public IActionResult DeleteItem(Guid id)
        {
            _termFeeItemManager.Delete(id);

            return Ok(ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}