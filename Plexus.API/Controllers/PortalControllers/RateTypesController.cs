using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.ViewModel.Payment;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class RateTypesController : BaseController
    {
        private readonly IRateTypeManager _rateTypeManager;

        public RateTypesController(IRateTypeManager rateTypeManager)
        {
            _rateTypeManager = rateTypeManager;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateRateTypeViewModel request)
        {
            var rateType = _rateTypeManager.Create(request);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, rateType));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchRateTypeCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var pagedRateType = _rateTypeManager.Search(parameters, page, pageSize);

            if (pagedRateType is null || !pagedRateType.Items.Any())
            {
                return StatusCode(204);
            }

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, pagedRateType));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var rateType = _rateTypeManager.GetById(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, rateType));
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] RateTypeViewModel request)
        {
            var rateType = _rateTypeManager.Update(id, request);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, rateType));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _rateTypeManager.Delete(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}